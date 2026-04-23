
using Products.Src.Application.Port.In;
using Products.Src.Application.Port.Out;
using Products.Src.Domain;

namespace Products.Src.Application.Service;

public class ProductService(IProductPort productPort) : IProductUseCase
{
    public async Task<int> CreateAsync(Product product)
    {
        return await productPort.CreateAsync(product);
    }

    public async Task<PagedResult<Product>> GetAllAsync(int page, int pageSize)
    {
        return await productPort.GetAllAsync(page, pageSize);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await productPort.GetByIdAsync(id);
    }

    public async Task UpdateAsync(int id, Product product)
    {
        await productPort.UpdateAsync(id, product);
    }

    public async Task DeleteAsync(int id)
    {
        await productPort.DeleteAsync(id);
    }
}