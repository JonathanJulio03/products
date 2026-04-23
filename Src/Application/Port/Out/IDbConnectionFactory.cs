using System.Data;

namespace Products.Src.Application.Port.Out;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}