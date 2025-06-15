using System.Collections.Generic;

namespace WeatherApp.Models
{
    public class WeatherModel
    {
        public string CityName { get; set; }
        public double Temperature { get; set; }
        public string Status { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double? Rain1h { get; set; }
        public int Cloudiness { get; set; }
        public double FeelsLike { get; set; }
        public string BackgroundImage { get; set; }
        public int Pressure { get; set; }
        
    }
}