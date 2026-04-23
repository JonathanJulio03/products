using FluentAssertions;
using Products.Src.Domain;
using Products.Src.Infrastructure.In.Rest.Command;
using Xunit;

namespace Products.Tests.Unit;

public class DomainAndDtosTests
{
    [Fact]
    public void ProductEntity_CanSetAndGetProperties()
    {
        var date = DateTime.Now;
        var product = new Product { Id = 1, Name = "Test", Description = "Desc", Price = 10, CreatedDate = date };
        
        product.Id.Should().Be(1);
        product.Name.Should().Be("Test");
        product.Description.Should().Be("Desc");
        product.Price.Should().Be(10);
        product.CreatedDate.Should().Be(date);
    }

    [Fact]
    public void ProductDtos_CanSetAndGetProperties()
    {
        var dto = new ProductDto { Name = "A", Description = "B", Price = 1 };
        dto.Name.Should().Be("A");
        
        var responseDto = new ProductResponseDto(1, "A", "B", 1, DateTime.Now);
        responseDto.Id.Should().Be(1);
    }
}