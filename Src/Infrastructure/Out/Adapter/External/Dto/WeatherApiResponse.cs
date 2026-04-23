using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.Src.Infrastructure.Out.Adapter.External.Dto;

public record WeatherApiResponse(
    [property: JsonPropertyName("location")] WeatherLocation? Location,
    [property: JsonPropertyName("current")] WeatherCurrent? Current
);