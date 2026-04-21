namespace Products.Api.Infrastructure.In.Rest.Command;

public record WeatherInfoDto(
    string City, 
    string Region, 
    string Country, 
    double TemperatureCelsius
);