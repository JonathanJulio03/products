using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Products.Src.Infrastructure.Exceptions;
using Products.Src.Infrastructure.Middleware;
using Xunit;

namespace Products.Tests.Unit.Infrastructure.Middleware;

public class GlobalExceptionMiddlewareTests
{
    private class TestDomainException(string message) : DomainException(message);

    [Fact]
    public async Task TryHandleAsync_OnDomainException_Returns400BadRequest()
    {
        var loggerMock = Substitute.For<ILogger<GlobalExceptionMiddleware>>();
        var sut = new GlobalExceptionMiddleware(loggerMock);
        
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream(); 
        
        var exception = new TestDomainException("Regla de negocio fallida");

        var handled = await sut.TryHandleAsync(context, exception, CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var jsonResponse = await new StreamReader(context.Response.Body).ReadToEndAsync();
        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        problemDetails.Should().NotBeNull();
        problemDetails!.Title.Should().Be("Error de business");
        problemDetails.Detail.Should().Be("Regla de negocio fallida");
    }
    [Fact]
    public async Task TryHandleAsync_OnKeyNotFoundException_Returns404NotFound()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<GlobalExceptionMiddleware>>();
        var sut = new GlobalExceptionMiddleware(loggerMock);
        var context = new DefaultHttpContext();
        
        // Act
        await sut.TryHandleAsync(context, new KeyNotFoundException("Id no existe"), CancellationToken.None);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Fact]
    public async Task TryHandleAsync_OnTechnicalException_Returns500InternalServerError()
    {
        // Arrange
        var loggerMock = Substitute.For<ILogger<GlobalExceptionMiddleware>>();
        var sut = new GlobalExceptionMiddleware(loggerMock);
        var context = new DefaultHttpContext();
        
        // Act
        await sut.TryHandleAsync(context, new TechnicalException("Db failure"), CancellationToken.None);

        // Assert
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public async Task TryHandleAsync_OnGenericException_Returns500InternalServerError()
    {
        var loggerMock = Substitute.For<ILogger<GlobalExceptionMiddleware>>();
        var sut = new GlobalExceptionMiddleware(loggerMock);
        var context = new DefaultHttpContext();
        
        await sut.TryHandleAsync(context, new Exception("Boom!"), CancellationToken.None);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}