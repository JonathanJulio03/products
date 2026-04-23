using System.Net;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Products.Src.Infrastructure.Exceptions;
using Products.Src.Infrastructure.Out.Adapter.External;
using Products.Src.Infrastructure.Out.Adapter.External.Dto;
using Xunit;

namespace Products.Tests.Unit.Infrastructure.Out;

public class WeatherAdapterTests
{
    private readonly IConfiguration _configMock;

    public WeatherAdapterTests()
    {
        _configMock = Substitute.For<IConfiguration>();
        _configMock["WeatherApi:Key"].Returns("fake-api-key");
    }

    private class MockHttpMessageHandler(HttpResponseMessage responseMessage) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            => Task.FromResult(responseMessage);
    }

    [Fact]
    public async Task GetByCity_WhenApiReturnsValidData_ReturnsWeatherInfoDto()
    {
        var apiResponse = new WeatherApiResponse(
            new WeatherLocation("London", "London Region", "UK"),
            new WeatherCurrent(15.5)
        );
        var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(apiResponse))
        };
        var httpClient = new HttpClient(new MockHttpMessageHandler(httpResponse)) 
        { 
            BaseAddress = new Uri("http://localhost/") 
        };
        var sut = new WeatherAdapter(httpClient, _configMock);

        var result = await sut.GetByCity("London");

        result.Should().NotBeNull();
        result!.City.Should().Be("London");
        result.TemperatureCelsius.Should().Be(15.5);
    }

    [Fact]
    public async Task GetByCity_WhenApiReturnsError500_ThrowsTechnicalExceptionForNetwork()
    {
        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
        var httpClient = new HttpClient(new MockHttpMessageHandler(httpResponse)) 
        { 
            BaseAddress = new Uri("http://localhost/") 
        };
        var sut = new WeatherAdapter(httpClient, _configMock);

        Func<Task> act = async () => await sut.GetByCity("Cucuta");

        await act.Should().ThrowAsync<TechnicalException>()
            .WithMessage("*Error de red al obtener el clima*");
    }

    [Fact]
    public async Task GetByCity_WhenUnexpectedErrorOccurs_ThrowsTechnicalExceptionForUnexpected()
    {
        var httpClient = Substitute.For<HttpClient>(); 
        var sut = new WeatherAdapter(httpClient, _configMock);

        Func<Task> act = async () => await sut.GetByCity("Cucuta");

        await act.Should().ThrowAsync<TechnicalException>()
            .WithMessage("*Error inesperado procesando el clima*");
    }
}