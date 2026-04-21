using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Products.Api.Infrastructure.Out.Adapter.External.Dto;

public record WeatherCurrent(
    [property: JsonPropertyName("temp_c")] double TempC
);