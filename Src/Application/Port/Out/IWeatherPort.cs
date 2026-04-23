using Products.Src.Infrastructure.In.Rest.Command;

namespace Products.Src.Application.Port.Out;

public interface IWeatherPort
{
    Task<WeatherInfoDto?> GetByCity(string city);
}