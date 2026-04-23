using FluentAssertions;
using NSubstitute;
using Products.Src.Application.Port.Out;
using Products.Src.Application.Service;
using Products.Src.Domain;
using Xunit;
using Bogus;

namespace Products.Tests.Unit.Application;

public class ProductServiceTests
{
    private readonly IProductPort _productPortMock;
    private readonly ProductService _sut;
    private readonly Faker<Product> _productFaker;

    public ProductServiceTests()
    {
        _productPortMock = Substitute.For<IProductPort>();
        _sut = new ProductService(_productPortMock);
        
        _productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000));
    }

    [Fact]
    public async Task CreateAsync_WhenCalledWithValidProduct_ReturnsNewId()
    {
        var newProduct = _productFaker.Generate();
        var expectedId = 99;
        _productPortMock.CreateAsync(newProduct).Returns(expectedId);

        var result = await _sut.CreateAsync(newProduct);

        result.Should().Be(expectedId);
        await _productPortMock.Received(1).CreateAsync(newProduct);
    }

    [Fact]
    public async Task GetAllAsync_WhenCalled_ReturnsPagedResult()
    {
        var products = _productFaker.Generate(3);
        var expectedPagedResult = new PagedResult<Product>(products, 3, 1, 10);
        _productPortMock.GetAllAsync(1, 10).Returns(expectedPagedResult);

        var result = await _sut.GetAllAsync(1, 10);

        result.Should().BeEquivalentTo(expectedPagedResult);
    }

    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProduct()
    {
        var expectedProduct = _productFaker.Generate();
        _productPortMock.GetByIdAsync(expectedProduct.Id).Returns(expectedProduct);

        var result = await _sut.GetByIdAsync(expectedProduct.Id);

        result.Should().BeEquivalentTo(expectedProduct);
    }

    [Fact]
    public async Task UpdateAsync_WhenCalled_DelegatesToPort()
    {
        var id = 1;
        var productToUpdate = _productFaker.Generate();

        await _sut.UpdateAsync(id, productToUpdate);

        await _productPortMock.Received(1).UpdateAsync(id, productToUpdate);
    }

    [Fact]
    public async Task DeleteAsync_WhenCalled_DelegatesToPort()
    {
        var id = 1;

        await _sut.DeleteAsync(id);

        await _productPortMock.Received(1).DeleteAsync(id);
    }
}