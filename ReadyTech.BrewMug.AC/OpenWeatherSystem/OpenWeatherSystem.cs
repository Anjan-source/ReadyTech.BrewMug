
namespace ReadyTech.BrewMug.AC.OpenWeatherSystem
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using ReadyTech.BrewMug.AC.common;
    using ReadyTech.BrewMug.AppMgr.common;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Models;
    using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
    using System.Threading.Tasks;

    /// <summary>
    /// This calss manages interaction to the targeted API and retreives the weather data
    /// </summary>
    public class OpenWeatherSystem : TargetApi, IOpenWeatherSystem
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenWeatherSystem> _logger;

        public OpenWeatherSystem(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<OpenWeatherSystem> logger) 
            : base(httpClientFactory, logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<RootObject> GetWheatherTemparatureAsync()
        {
            var externalService = _configuration.GetSection("ExternalServices").Get<ExternalServices>();
            string requestURI = $"{externalService.WeatherApiUrl}{externalService.City}&appid={externalService.ApiKey}";

            var weatherResponse = await GetAsync<RootObject>(requestURI);

            return weatherResponse;
        }
    }
}
