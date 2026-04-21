using System.Data;
using Products.Api.Application.Port.Out;
using Products.Api.Infrastructure.Exceptions;
using Microsoft.Data.SqlClient;

namespace Products.Api.Infrastructure.Out.Adapter.Persistence.Data;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new TechnicalException("Cadena de conexión no encontrada");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}