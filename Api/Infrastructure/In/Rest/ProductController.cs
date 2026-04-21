using Products.Api.Application.Port.In;
using Microsoft.AspNetCore.Mvc;

using Mapster;
using Products.Api.Domain;
using Products.Api.Infrastructure.In.Rest.Command;

namespace Products.Api.Infrastructure.In.Rest;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductUseCase productUseCase) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagedData = await productUseCase.GetAllAsync(page, pageSize);
        
        return Ok(pagedData);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await productUseCase.GetByIdAsync(id);
        if (product is null) return NotFound();

        return Ok(product.Adapt<ProductResponseDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        var productToCreate = dto.Adapt<Product>();
        
        var newProductId = await productUseCase.CreateAsync(productToCreate);
        return CreatedAtAction(nameof(GetById), new { id = newProductId }, new { Id = newProductId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
    {
        var productToUpdate = dto.Adapt<Product>();
        
        await productUseCase.UpdateAsync(id, productToUpdate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await productUseCase.DeleteAsync(id);
        return NoContent();
    }
}