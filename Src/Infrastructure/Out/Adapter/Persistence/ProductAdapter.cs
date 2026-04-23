using System.Data;
using Products.Src.Application.Port.Out;
using Products.Src.Domain;
using Products.Src.Infrastructure.Exceptions;
using Dapper;

namespace Products.Src.Infrastructure.Out.Adapter.Persistence;

public class ProductAdapter(IDbConnectionFactory connectionFactory) : IProductPort
{
    public Task<PagedResult<Product>> GetAllAsync(int page, int pageSize) =>
        ExecuteAsync("Technical error while trying to retrieve the paginated product list.",
            async connection =>
            {
                var parameters = new { PageNumber = page, PageSize = pageSize };

                using var multi = await connection.QueryMultipleAsync(
                    "get_all_products",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var totalRecords = await multi.ReadFirstAsync<int>();
                var items = await multi.ReadAsync<Product>();

                return new PagedResult<Product>(items, totalRecords, page, pageSize);
            });

    public Task<Product?> GetByIdAsync(int id) =>
        ExecuteAsync($"Technical error while trying to retrieve the product with ID {id}.",
            connection => connection.QueryFirstOrDefaultAsync<Product>(
                "get_product_by_id",
                new { id },
                commandType: CommandType.StoredProcedure));

    public Task<int> CreateAsync(Product product) =>
        ExecuteAsync("Technical error while trying to create a new product in the database.",
            async connection =>
            {
                var parameters = BuildBaseProductParameters(product);

                parameters.Add("@new_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await connection.ExecuteAsync(
                    "create_product",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return parameters.Get<int>("@new_id");
            });

    public Task UpdateAsync(int id, Product product) =>
        ExecuteAsync($"Technical error while trying to update the product with ID {id}.",
            connection =>
            {
                var parameters = BuildBaseProductParameters(product);

                parameters.Add("@id", id);

                return connection.ExecuteAsync(
                    "update_product",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
            });

    public Task DeleteAsync(int id) =>
        ExecuteAsync($"Technical error while trying to delete the product with ID {id}.",
            connection => connection.ExecuteAsync(
                "delete_product",
                new { id },
                commandType: CommandType.StoredProcedure));

    private async Task<T> ExecuteAsync<T>(string errorMessage, Func<IDbConnection, Task<T>> queryFunc)
    {
        try
        {
            using var connection = connectionFactory.CreateConnection();
            return await queryFunc(connection);
        }
        catch (Exception ex)
        {
            throw new TechnicalException(errorMessage, ex);
        }
    }

    private static DynamicParameters BuildBaseProductParameters(Product product)
    {
        var parameters = new DynamicParameters();

        parameters.Add("name", product.Name);
        parameters.Add("description", product.Description);
        parameters.Add("price", product.Price);

        return parameters;
    }
}