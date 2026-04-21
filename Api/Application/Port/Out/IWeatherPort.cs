using Products.Api.Infrastructure.In.Rest.Command;

namespace Products.Api.Application.Port.Out;

public interface IWeatherPort
{
    Task<WeatherInfoDto?> GetByCity(string city);
}