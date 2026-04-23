using Products.Src.Domain;
using Products.Src.Infrastructure.In.Rest.Command;

namespace Products.Src.Application.Port.In;

public interface IWeatherUseCase
{
    Task<WeatherInfoDto?> GetByCity(string city);
}