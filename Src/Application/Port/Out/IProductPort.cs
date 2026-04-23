
using Products.Src.Domain;

namespace Products.Src.Application.Port.Out;

public interface IProductPort
{
    Task<PagedResult<Product>> GetAllAsync(int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task<int> CreateAsync(Product product);
    Task UpdateAsync(int id, Product product);
    Task DeleteAsync(int id);
}