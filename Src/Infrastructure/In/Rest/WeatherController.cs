using Products.Src.Application.Port.In;
using Microsoft.AspNetCore.Mvc;

namespace Products.Src.Infrastructure.In.Rest;

[ApiController]
[Route("api/[controller]")]
public class WeatherController(IWeatherUseCase weatherUseCase) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetByCity([FromQuery] string city)
    {
        var weatherInfoDto = await weatherUseCase.GetByCity(city);
        if (weatherInfoDto is null) return NotFound();

        return Ok(weatherInfoDto);
    }
}