using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSEServiceClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BSEMFService.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        MFUploadServiceClient objClient = new BSEServiceClient.MFUploadServiceClient(MFUploadServiceClient.EndpointConfiguration.WSHttpBinding_IMFUploadService1);
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {


            var status=CreateUCC();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [NonAction]
        public async Task<string> generatePassCodeAsync()
        {
            var passkey = "test" + DateTime.Now.Second;
           
            String password = await objClient.getPasswordAsync("2008101", "20081", "123456", passkey);
            return password;
        }
       
        [NonAction]
        public async Task<string> CreateUCC()
        {
            string pwd = await generatePassCodeAsync();
            string[] pass = pwd.Split('|');
            string Param = "RK0019|SI|01|08|RAHUL K CHAUHAN|||18/04/1984|M||AQBPP8764H||||P||||||SB|025101502569|560229016|ICIC0000251|Y||||||||||||||||||||||NO 1102/A 5TH CROSS ST. MARYS CONVENT RO|TD HALLI BENGALURU (NSUNMDS)||BANGALORE|KA|560057|INDIA|||||deepukumarp@gmail.com|M|02|||||||||||||||8586984034";
            string result= await objClient.MFAPIAsync("02", "2008101", pass[1].ToString(), Param);
            return result;
        }
    }
}
