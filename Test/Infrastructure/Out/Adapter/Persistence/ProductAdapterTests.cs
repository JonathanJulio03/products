using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using NSubstitute;
using Products.Src.Application.Port.Out;
using Products.Src.Domain;
using Products.Src.Infrastructure.Out.Adapter.Persistence;
using Testcontainers.MsSql;
using Xunit;

namespace Products.Tests.Integration.Infrastructure.Out;

public class ProductAdapterTests : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;
    private ProductAdapter _sut = null!;

    public ProductAdapterTests()
    {
        _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();            
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var connectionString = _msSqlContainer.GetConnectionString();
        using var connection = new SqlConnection(connectionString);
        await connection.ExecuteAsync(@"
            CREATE TABLE products (
                id INT IDENTITY(1,1) PRIMARY KEY,
                name NVARCHAR(150), description NVARCHAR(500), price DECIMAL(18,2)
            );
            EXEC('CREATE PROCEDURE create_product @name NVARCHAR(150), @description NVARCHAR(500), @price DECIMAL(18,2), @new_id INT OUTPUT AS BEGIN INSERT INTO products (name, description, price) VALUES (@name, @description, @price); SET @new_id = SCOPE_IDENTITY(); END;');
        ");

        var factoryMock = Substitute.For<IDbConnectionFactory>();
        factoryMock.CreateConnection().Returns(_ => new SqlConnection(connectionString));

        _sut = new ProductAdapter(factoryMock);
    }

    [Fact]
    public async Task CreateAsync_WhenValidProduct_InsertsIntoRealDatabaseAndReturnsId()
    {
        var product = new Product { Name = "Integration Test", Description = "Desc", Price = 99.99m };

        var resultId = await _sut.CreateAsync(product);

        resultId.Should().BeGreaterThan(0);
        
        using var connection = new SqlConnection(_msSqlContainer.GetConnectionString());
        var inserted = await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM products WHERE id = @id", new { id = resultId });
        inserted.Should().NotBeNull();
        inserted!.Name.Should().Be("Integration Test");
    }

    public async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}