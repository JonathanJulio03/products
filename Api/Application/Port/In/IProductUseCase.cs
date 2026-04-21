using Products.Api.Domain;

namespace Products.Api.Application.Port.In;

public interface IProductUseCase
{
    Task<int> CreateAsync(Product product);
    Task<PagedResult<Product>> GetAllAsync(int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task UpdateAsync(int id, Product product);
    Task DeleteAsync(int id);
}