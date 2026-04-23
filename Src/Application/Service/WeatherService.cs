
using Products.Src.Application.Port.In;
using Products.Src.Application.Port.Out;
using Products.Src.Infrastructure.In.Rest.Command;

namespace Products.Src.Application.Service;

public class WeatherService(IWeatherPort weatherPort) : IWeatherUseCase
{
    public Task<WeatherInfoDto?> GetByCity(string city)
    {
        return weatherPort.GetByCity(city);
    }
}