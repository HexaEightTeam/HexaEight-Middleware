using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace HexaEight_Middleware_SampleDemo.Controllers
{

    public class incomingjson
    {
        public List<string> location { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };


        private static readonly string[] locations = new[]
        {
            "Aberdeen", "Anacortes","Arlington", "Auburn","Battle Ground","Bellevue","Bellingham","Bonney Lake","Bothell","Bremerton","Burien"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                isauthenticated = false,
                loggedinuser = "",
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                location = locations[rng.Next(locations.Length)],
                 Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("/api/getweather/{resource}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IEnumerable<WeatherForecast> Get(string resource)
        {
            var rng = new Random();
                return Enumerable.Range(1, 10).Select(index => new WeatherForecast
                {
                    isauthenticated = true,
                    loggedinuser = HttpContext.User.Identity.Name.ToString(),
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    location = resource,
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
        }


        [HttpPost("/api/fetchcurrentweather")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IEnumerable<WeatherForecast>> Post(string resource)
        {

            string body = "";
            using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
            {
                body = await stream.ReadToEndAsync();
            }
            body = body.ToString().TrimEnd('\0').Trim();

            var data = JsonConvert.DeserializeObject<dynamic>(body);
            var postdata = System.Text.Json.JsonSerializer.Deserialize<incomingjson>(data);
            int cnt = postdata.location.Count;

            var rng = new Random();
            return Enumerable.Range(1, cnt).Select(index => new WeatherForecast
            {
                isauthenticated = true,
                loggedinuser = HttpContext.User.Identity.Name.ToString(),
                Date = DateTime.Now,
                TemperatureC = rng.Next(-20, 55),
                location = postdata.location[index-1].ToString(),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpGet]
        [Route("/api/hello")]
        [Authorize(AuthenticationSchemes = "Bearer")]

        public FileStreamResult GetTest()
        {
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("Hello " + HttpContext.User.Identity.Name.ToString()));
            return new FileStreamResult(stream, new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("text/plain"))
            {
                FileDownloadName = "test.txt"
            };
        }

    }
}
