using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExternalWeatherForecastController : ControllerBase
{
    [HttpGet(Name = "GetExternalWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // Call to external service on localhost:5001/WeatherForecast and return the result
        var client = new HttpClient();
        var response = client.GetAsync("https://localhost:7129/WeatherForecast").Result;
        var result = response.Content.ReadAsStringAsync().Result;
        return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(result);
    }
}
