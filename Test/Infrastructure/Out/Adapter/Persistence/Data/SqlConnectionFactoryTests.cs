using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Products.Src.Infrastructure.Exceptions;
using Products.Src.Infrastructure.Out.Adapter.Persistence.Data;
using Xunit;

namespace Products.Tests.Unit.Infrastructure.Out.Data;

public class SqlConnectionFactoryTests
{
    [Fact]
    public void CreateConnection_WithValidConfig_ReturnsSqlConnection()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"ConnectionStrings:DefaultConnection", "Server=myServer;Database=myDb;"}
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var sut = new SqlConnectionFactory(configuration);

        var connection = sut.CreateConnection();

        connection.Should().NotBeNull();
        connection.Should().BeOfType<SqlConnection>();
    }

    [Fact]
    public void Constructor_WithMissingConfig_ThrowsTechnicalException()
    {
        IConfiguration configuration = new ConfigurationBuilder().Build();

        Action act = () => new SqlConnectionFactory(configuration);

        act.Should().Throw<TechnicalException>()
           .WithMessage("Cadena de conexión no encontrada");
    }
}