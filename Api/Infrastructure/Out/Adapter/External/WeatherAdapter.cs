using System.Net.Http.Json;
using Products.Api.Application.Port.Out;
using Products.Api.Infrastructure.Exceptions;
using Products.Api.Infrastructure.In.Rest.Command;
using Products.Api.Infrastructure.Out.Adapter.External.Dto;

namespace Products.Api.Infrastructure.Out.Adapter.External;

public class WeatherAdapter(HttpClient httpClient, IConfiguration configuration) : IWeatherPort
{
    public async Task<WeatherInfoDto?> GetByCity(string city)
    {
        try
        {
            var apiKey = configuration["WeatherApi:Key"];

            var response = await httpClient.GetFromJsonAsync<WeatherApiResponse>(
                $"current.json?key={apiKey}&q={city}");
            
            if (response?.Location == null || response?.Current == null)
                return null;

            return new WeatherInfoDto(
                response.Location.Name,
                response.Location.Region,
                response.Location.Country,
                response.Current.TempC
            );
        }
        catch (HttpRequestException ex)
        {
            throw new TechnicalException($"Error de red al obtener el clima para '{city}'.", ex);
        }
        catch (Exception ex)
        {
            throw new TechnicalException($"Error inesperado procesando el clima de '{city}'.", ex);
        }
    }
}