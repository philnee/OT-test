using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        WeatherForecast[] weatherForecasts;
        using (var firstActivity = DiagnosticsConfig.ActivitySource.StartActivity("First Activity"))
        {
            // Do some stuff in here
        }

        using (var createForecasts = DiagnosticsConfig.ActivitySource.StartActivity("Create Forecasts"))
        {
            weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
            // Do some stuff in here
        }

        using (var postActivity = DiagnosticsConfig.ActivitySource.StartActivity("Slow Activity"))
        {
            DoSlowStuff(postActivity);
        }
        
        return weatherForecasts;
    }

    private void DoSlowStuff(Activity? postActivity)
    {
        var millisecondsTimeout = new Random().Next(3000);
        postActivity?.AddTag("delay", millisecondsTimeout);
        Thread.Sleep(millisecondsTimeout);
    }
}
