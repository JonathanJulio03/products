using Products.Api.Domain;
using Products.Api.Infrastructure.In.Rest.Command;

namespace Products.Api.Application.Port.In;

public interface IWeatherUseCase
{
    Task<WeatherInfoDto?> GetByCity(string city);
}