
using Products.Api.Application.Port.In;
using Products.Api.Application.Port.Out;
using Products.Api.Infrastructure.In.Rest.Command;

namespace Products.Api.Application.Service;

public class WeatherService(IWeatherPort weatherPort) : IWeatherUseCase
{
    public Task<WeatherInfoDto?> GetByCity(string city)
    {
        return weatherPort.GetByCity(city);
    }
}