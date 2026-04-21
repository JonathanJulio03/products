using System.Data;

namespace Products.Api.Application.Port.Out;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}