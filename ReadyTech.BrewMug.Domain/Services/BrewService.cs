
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
        private static int _requestCount = 0;
        public static class _utcTime
        {
            public static Func<DateTimeOffset> Now = () => DateTimeOffset.UtcNow;
        }
        public readonly IMapper _mapper;
        public readonly ILogger<BrewService> _logger;
        public readonly IBrewRepository _brewRepository;
        public BrewService(IBrewRepository brewRepository, IMapper mapper, ILogger<BrewService> logger) {
        
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

            _requestCount++;
            var utcDateTime = _utcTime.Now();

            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new BrewCoffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.UtcNow
                },
                CoffeStatus = (utcDateTime.Month == 4 && utcDateTime.Day == 1)
                    ? CoffeeStatus.Teapot
        :           (_requestCount == 5)
                        ? CoffeeStatus.ServiceUnavailable
                        :CoffeeStatus.Ready
            };

            //reset the count
            if(_requestCount == 5) 
            { 
                _requestCount = 0; 
            }
            return brewCoffeeVm;

        }
    }
}
