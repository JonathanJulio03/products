using Products.Api.Domain;

namespace Products.Api.Application.Port.In;

public interface IProductUseCase
{
    Task<int> CreateAsync(Product product);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task UpdateAsync(int id, Product product);
    Task DeleteAsync(int id);
}