using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Products.Src.Application.Port.In;
using Products.Src.Infrastructure.In.Rest;
using Products.Src.Infrastructure.In.Rest.Command;
using Xunit;

namespace Products.Tests.Unit.Infrastructure.In;

public class WeatherControllerTests
{
    private readonly IWeatherUseCase _weatherUseCaseMock;
    private readonly WeatherController _sut;

    public WeatherControllerTests()
    {
        _weatherUseCaseMock = Substitute.For<IWeatherUseCase>();
        _sut = new WeatherController(_weatherUseCaseMock);
    }

    [Fact]
    public async Task GetByCity_WhenCityExists_ReturnsOk()
    {
        var expectedWeather = new WeatherInfoDto("Cucuta", "NS", "Colombia", 32.0);
        _weatherUseCaseMock.GetByCity("Cucuta").Returns(expectedWeather);

        var result = await _sut.GetByCity("Cucuta");

        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expectedWeather);
    }

    [Fact]
    public async Task GetByCity_WhenCityNotFound_ReturnsNotFound()
    {
        _weatherUseCaseMock.GetByCity("Unknown").ReturnsNull();

        var result = await _sut.GetByCity("Unknown");

        result.Should().BeOfType<NotFoundResult>();
    }
}