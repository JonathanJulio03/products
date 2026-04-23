using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Products.Src.Application.Port.Out;
using Products.Src.Application.Service;
using Products.Src.Infrastructure.In.Rest.Command;
using Xunit;

namespace Products.Tests.Unit.Application.Service;

public class WeatherServiceTests
{
    private readonly IWeatherPort _weatherPortMock;
    private readonly WeatherService _sut;

    public WeatherServiceTests()
    {
        _weatherPortMock = Substitute.For<IWeatherPort>();
        _sut = new WeatherService(_weatherPortMock);
    }

    [Fact]
    public async Task GetByCity_WhenCityExists_ReturnsWeatherInfoDto()
    {
        var city = "Cucuta";
        var expectedDto = new WeatherInfoDto(city, "North Santander", "Colombia", 32.5);
        _weatherPortMock.GetByCity(city).Returns(expectedDto);

        var result = await _sut.GetByCity(city);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedDto);
        
        await _weatherPortMock.Received(1).GetByCity(city);
    }

    [Fact]
    public async Task GetByCity_WhenCityDoesNotExist_ReturnsNull()
    {
        var city = "Atlantis";
        _weatherPortMock.GetByCity(city).ReturnsNull();

        var result = await _sut.GetByCity(city);

        result.Should().BeNull();
        await _weatherPortMock.Received(1).GetByCity(city);
    }
}