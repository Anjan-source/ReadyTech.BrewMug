
namespace ReadyTech.BrewMug.AppMgr.queries.BrewCoffee
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ReadyTech.BrewMug.AppMgr.common;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Models;

    /// <summary>
    /// GetBrewCoffeeVmQueryHandler will manage request handler operation to get BrewCoffee Information
    /// </summary>
    public class GetBrewCoffeeVmQueryHandler : IRequestHandler<GetBrewCoffeeVmQuery, BrewCoffeeVm>
    {
        //private readonly IMapper _mapper;
        private readonly ILogger<GetBrewCoffeeVmQueryHandler> _logger;
        private readonly IBrewService _brewService;
        private readonly ExternalServices _externalServices;
        private readonly IOpenWeatherSystem _openWeatherSystem;
        public GetBrewCoffeeVmQueryHandler(IOpenWeatherSystem openWeatherSystem, IBrewService brewService, ILogger<GetBrewCoffeeVmQueryHandler> logger)
        {
            //  _mapper = mapper;
            _openWeatherSystem = openWeatherSystem;
            _logger = logger;
            _brewService = brewService;
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
                var weatherResponse = await _openWeatherSystem.GetWheatherTemparatureAsync();

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
