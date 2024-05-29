
namespace ReadyTech.BrewMug.AppMgr.queries.BrewCoffee
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using ReadyTech.BrewMug.AppMgr.Interfaces;

    /// <summary>
    /// GetBrewCoffeeVmQueryHandler will manage request handler operation to get BrewCoffee Information
    /// </summary>
    public class GetBrewCoffeeVmQueryHandler : IRequestHandler<GetBrewCoffeeVmQuery, BrewCoffeeVm>
    {
       // private readonly IMapper _mapper;
        private readonly ILogger<GetBrewCoffeeVmQueryHandler> _logger;
        private readonly IBrewService _brewService;
        public GetBrewCoffeeVmQueryHandler(IBrewService brewService, ILogger<GetBrewCoffeeVmQueryHandler> logger)
        {
            _logger = logger;
            _brewService = brewService;
        }

        /// <summary>
        /// Handler will communicate with Domain services and retrive the requested information
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns> Brew Coffee information Message and Prepared date time</returns>
        public async Task<BrewCoffeeVm> Handle(GetBrewCoffeeVmQuery request, CancellationToken cancellationToken)
        {
            try 
            {
                return  await _brewService.GetBrewCoffeeInformationAsync();

            }catch(Exception ex)
            {
                _logger.LogError("Error occure in the Request Handler {message}" + ex.Message);
                return null;
            }
        }
    }
}
