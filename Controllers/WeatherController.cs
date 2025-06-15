using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Controllers
{
    public class WeatherController : Controller
    {
        private readonly string apiKey = "7b5a17555d3053946fa4366f8de2c9a8";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city)
        {
            if (string.IsNullOrEmpty(city))
                return View();

            // 1. Lấy tọa độ thành phố
            string geoUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={apiKey}";
            using (var client = new HttpClient())
            {
                var geoResponse = await client.GetStringAsync(geoUrl);
                var geoArray = JArray.Parse(geoResponse);
                if (geoArray.Count == 0)
                {
                    ViewBag.Error = "Không tìm thấy thành phố.";
                    return View();
                }
                var lat = geoArray[0]["lat"];
                var lon = geoArray[0]["lon"];

                // 2. Lấy thông tin thời tiết
                string weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&units=metric&lang=vi&appid={apiKey}";
                var weatherResponse = await client.GetStringAsync(weatherUrl);
                var weatherJson = JObject.Parse(weatherResponse);

                var model = new WeatherModel
                {
                    CityName = (string)weatherJson["name"],
                    Temperature = (double)weatherJson["main"]["temp"],
                    Status = (string)weatherJson["weather"][0]["description"],
                    TempMin = (double)weatherJson["main"]["temp_min"],
                    TempMax = (double)weatherJson["main"]["temp_max"],
                    Humidity = (int)weatherJson["main"]["humidity"],
                    WindSpeed = (double)weatherJson["wind"]["speed"],
                    Rain1h = weatherJson["rain"]?["1h"] != null ? (double?)weatherJson["rain"]["1h"] : null,
                    Cloudiness = (int)weatherJson["clouds"]["all"],
                    FeelsLike = (double)weatherJson["main"]["feels_like"],
                    BackgroundImage = weatherJson["weather"][0]["main"].ToString().ToLower().Contains("rain") ? "/images/rainy-bg.jpg" : "/images/sunny-bg.jpg"
                };

                return View(model);
            }
        }
    }
}