
namespace ReadyTech.BrewMug.Domain.Services
{
    using AutoMapper;
    using Microsoft.Extensions.Logging;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Enums;
    using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
    using ReadyTech.BrewMug.Data.Interfaces;
    /// <summary>
    /// Brew Service manages all the business logic and returns the expected results
    /// </summary>
    public class BrewService : IBrewService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BrewService> _logger;
        private readonly IBrewRepository _brewRepository;
        private readonly RequestService _requestService;
        public BrewService(RequestService requestService, IBrewRepository brewRepository, IMapper mapper, ILogger<BrewService> logger) {
        
            _requestService = requestService;
            _mapper = mapper;
            _logger = logger;
            _brewRepository = brewRepository;
        }
       
        /// <summary>
        /// Manages business logic and communicates with Repository to get data from DB
        /// Also, once get data, map with AppMgr model and return
        /// </summary>
        /// <returns></returns>
        public async Task<BrewCoffeeVm> GetBrewCoffeeInformationAsync()
        {
            //if we need to get values from Database use reposiotory (Dapper/Entity) to get Data set

            //var result= _brewRepository.GetBrewCoffeeAsync().ConfigureAwait(false);
            //var brewCoffee=_mapper.Map<BrewCoffeeVm>(result);
            
            _requestService.IncrementRequestCount();
            var utcDateTime = _requestService.UtcDateTime;

            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new BrewCoffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.UtcNow
                },
                CoffeStatus = (utcDateTime.Month == 4 && utcDateTime.Day == 1)
                    ? CoffeeStatus.Teapot
        :           (_requestService.RequestCount == 5)
                        ? CoffeeStatus.ServiceUnavailable
                        :CoffeeStatus.Ready
            };

            //reset the count
            if(_requestService.RequestCount == 5) 
            {
                _requestService.ResetRequestCount();
            }
            return brewCoffeeVm;

        }
    }
}
