using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.Src.Infrastructure.Out.Adapter.External.Dto;

public record WeatherLocation(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("region")] string Region,
    [property: JsonPropertyName("country")] string Country
);