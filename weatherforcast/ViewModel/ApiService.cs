using System.Text.Json;
using WeatherApp.Model;

namespace WeatherApp.ViewModel;

public class ApiService
{
    public static async Task<Root> GetWeatherByCity(string city)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync
            (
                string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid=8604f6586c221d18b469b5a0b24d246a", city)
            );
        var weatherData = JsonSerializer.Deserialize<Root>(response)    ?? throw new Exception("Failed to do it");
        return weatherData;
    }

    public static async Task<Root> GetWeather(double latitude, double longitude)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetStringAsync
            (
                string.Format("https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=8604f6586c221d18b469b5a0b24d246a", latitude, longitude)
            );
        var weatherData = JsonSerializer.Deserialize<Root>(response)    ?? throw new Exception("Failed to do it");
        return weatherData;
    }
}
