using System.Text.Json;

namespace FrontWasm.Components
{
    public interface IWeatherForecastService
    {
        WeatherForecast[]? GetForecast();

        WeatherForecast[]? GetForecast(DateTime dateTime);

        Task<WeatherForecast[]?> GetForecastAsync();

        Task<WeatherForecast[]?> GetForecastAsync(DateTime dateTime);
    }

    public class WeatherForecastService : IWeatherForecastService
    {
        private const string weatherJson = $@"[
              {{
                ""date"": ""2022-01-06"",
                ""temperatureC"": 1,
                ""summary"": ""Freezing""
              }},
              {{
                ""date"": ""2022-01-07"",
                ""temperatureC"": 14,
                ""summary"": ""Bracing""
              }},
              {{
                ""date"": ""2022-01-08"",
                ""temperatureC"": -13,
                ""summary"": ""Freezing""
              }},
              {{
                ""date"": ""2022-01-09"",
                ""temperatureC"": -16,
                ""summary"": ""Balmy""
              }},
              {{
                ""date"": ""2022-01-10"",
                ""temperatureC"": -2,
                ""summary"": ""Chilly""
              }}
            ]
            ";
        private readonly WeatherForecast[]? forecasts;

        public WeatherForecastService()
        {
            forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(weatherJson);
        }

        public WeatherForecast[]? GetForecast() => forecasts;

        public WeatherForecast[]? GetForecast(DateTime dateTime)
        {
            return forecasts?.Where(x => x.Equals(dateTime))
                .ToArray();
        }

        public async Task<WeatherForecast[]?> GetForecastAsync()
        {
            WeatherForecast[]? result = null;

            await Task.Run(() =>
            {
                result = GetForecast();
            });

            return result;
        }

        public async Task<WeatherForecast[]?> GetForecastAsync(DateTime dateTime)
        {
            WeatherForecast[]? result = null;

            await Task.Run(() =>
            {
                result = GetForecast(dateTime);
            });

            return result;
        }
    }

    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
