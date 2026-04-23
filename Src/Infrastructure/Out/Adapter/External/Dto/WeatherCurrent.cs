using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.Src.Infrastructure.Out.Adapter.External.Dto;

public record WeatherCurrent(
    [property: JsonPropertyName("temp_c")] double TempC
);