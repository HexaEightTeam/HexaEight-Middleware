using System;

namespace HexaEight_Middleware_SampleDemo
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
        
        public bool isauthenticated { get; set; }
        public string loggedinuser { get; set; }
        public string location { get; set; }


    }
}
