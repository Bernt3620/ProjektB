using System.Collections.Concurrent;
using Newtonsoft.Json;
using Assignment_A1_03.Models;

namespace Assignment_A1_03.Services;

public class OpenWeatherService
{
    readonly HttpClient _httpClient = new HttpClient();
    readonly ConcurrentDictionary<(double, double, string), (Forecast, DateTime)> _cachedGeoForecasts = new();
    readonly ConcurrentDictionary<(string, string), (Forecast, DateTime)> _cachedCityForecasts = new();

    // Your API key
    readonly string apiKey = "718d1dae0a5ecbcc44ef499324b4c51b"; // Add the API key here.

    // Event declaration
    public event EventHandler<string> WeatherForecastAvailable;

    protected virtual void OnWeatherForecastAvailable(string message)
    {
        WeatherForecastAvailable?.Invoke(this, message);
    }

    public async Task<Forecast> GetForecastAsync(string city)
    {
        var cacheKey = (city, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        if (_cachedCityForecasts.TryGetValue(cacheKey, out var cachedEntry) &&
            DateTime.Now - cachedEntry.Item2 < TimeSpan.FromMinutes(1))
        {
            OnWeatherForecastAvailable($"Forecast for {city} retrieved from cache.");
            return cachedEntry.Item1;
        }

        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);
        _cachedCityForecasts[cacheKey] = (forecast, DateTime.Now);

        OnWeatherForecastAvailable($"Forecast for {city} retrieved from API.");
        return forecast;
    }

    public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
    {
        var cacheKey = (latitude, longitude, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        if (_cachedGeoForecasts.TryGetValue(cacheKey, out var cachedEntry) &&
            DateTime.Now - cachedEntry.Item2 < TimeSpan.FromMinutes(1))
        {
            OnWeatherForecastAvailable($"Forecast for coordinates ({latitude}, {longitude}) retrieved from cache.");
            return cachedEntry.Item1;
        }

        var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

        Forecast forecast = await ReadWebApiAsync(uri);
        _cachedGeoForecasts[cacheKey] = (forecast, DateTime.Now);

        OnWeatherForecastAvailable($"Forecast for coordinates ({latitude}, {longitude}) retrieved from API.");
        return forecast;
    }

    private async Task<Forecast> ReadWebApiAsync(string uri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        WeatherApiData wd = JsonConvert.DeserializeObject<WeatherApiData>(content);

        var forecast = new Forecast
        {
            City = wd.city.name,
            Items = wd.list.Select(item => new ForecastItem
            {
                DateTime = UnixTimeStampToDateTime(item.dt),
                Temperature = item.main.temp,
                WindSpeed = item.wind.speed,
                Description = item.weather.First().description,
                Icon = $"http://openweathermap.org/img/w/{item.weather.First().icon}.png"
            }).ToList()
        };

        return forecast;
    }

    private DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
        DateTime.UnixEpoch.AddSeconds(unixTimeStamp).ToLocalTime();
}
