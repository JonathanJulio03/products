using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Products.Src.Application.Port.In;
using Products.Src.Domain;
using Products.Src.Infrastructure.In.Rest;
using Products.Src.Infrastructure.In.Rest.Command;
using Xunit;

namespace Products.Tests.Unit.Infrastructure.In;

public class ProductControllerTests
{
    private readonly IProductUseCase _productUseCaseMock;
    private readonly ProductController _sut;

    public ProductControllerTests()
    {
        _productUseCaseMock = Substitute.For<IProductUseCase>();
        _sut = new ProductController(_productUseCaseMock);
    }

    [Fact]
    public async Task GetById_WhenProductExists_ReturnsOkWithDto()
    {
        var productId = 1;
        var existingProduct = new Product { Id = productId, Name = "Test" };
        _productUseCaseMock.GetByIdAsync(productId).Returns(existingProduct);

        var result = await _sut.GetById(productId);

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var responseDto = okResult.Value.Should().BeOfType<ProductResponseDto>().Subject;
        responseDto.Id.Should().Be(productId);
    }

    [Fact]
    public async Task GetById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var productId = 99;
        _productUseCaseMock.GetByIdAsync(productId).ReturnsNull();

        var result = await _sut.GetById(productId);

        result.Should().BeOfType<NotFoundResult>();
    }
}