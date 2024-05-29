
namespace ReadyTech.BrewMug.AppMgr.queries.BrewCoffee
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using ReadyTech.BrewMug.AC.OpenWeatherSystem;
    using ReadyTech.BrewMug.AppMgr.common;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Models;

    /// <summary>
    /// GetBrewCoffeeVmQueryHandler will manage request handler operation to get BrewCoffee Information
    /// </summary>
    public class GetBrewCoffeeVmQueryHandler : TargetApi, IRequestHandler<GetBrewCoffeeVmQuery, BrewCoffeeVm>
    {
        private readonly IMapper _mapper;
        private readonly ILogger<GetBrewCoffeeVmQueryHandler> _logger;
        private readonly IBrewService _brewService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ExternalServices _externalServices;
        private readonly IConfiguration _configuration;
        public GetBrewCoffeeVmQueryHandler(IConfiguration configuration, IBrewService brewService, IMapper mapper, ILogger<GetBrewCoffeeVmQueryHandler> logger, IHttpClientFactory httpClientFactory)
            : base(httpClientFactory, logger)
        {
            _mapper = mapper;
            _logger = logger;
            _brewService = brewService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        /// <summary>
        /// Handler will communicate with Domain services and retrive the requested information
        /// Interacts with Openweather API from AC layer and retrieves city temparature
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> Brew Coffee information Message and Prepared date time</returns>
        public async Task<BrewCoffeeVm> Handle(GetBrewCoffeeVmQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var externalService = _configuration.GetSection("ExternalServices").Get<ExternalServices>();
                string requestURI = $"{externalService.WeatherApiUrl}{externalService.City}&appid={externalService.ApiKey}";

                var weatherResponse = await GetAsync<RootObject>(requestURI);

                double tempK = weatherResponse.main.temp;
                double tempC = tempK - 273.15;
                var response = await _brewService.GetBrewCoffeeInformationAsync();

                if (weatherResponse != null && tempC > 30)
                {
                    response.BrewCoffeeInfo.Message = "Your refreshing iced coffee is ready";
                }

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured in the Request Handler {message}" + ex.Message);
                return null;
            }
        }
    }
}
