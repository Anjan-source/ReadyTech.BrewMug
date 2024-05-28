
namespace ReadyTech.BrewMug.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using ReadyTech.BrewMug.AppMgr.Enums;
    using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;

    /// <summary>
    /// Brew controller responsible for managing BrewCoffee related operations
    /// </summary>
    [ApiController]
    [Route("brew-coffee")]
    public class BrewController : BaseController
    {
        private readonly ILogger<BrewController> _logger;

        public BrewController(IMediator mediator, ILogger<BrewController> logger):base(mediator)
        {
            _logger = logger;
        }

        /// <summary>
        /// Retrives information about Brew Coffee 
        /// </summary>
        /// <returns>{BrewCoffeeVm} model represts message and prepared datetime</returns>
        [HttpGet]
        public async Task<IActionResult> GetBrewCoffeeAsync()
        {
            var response = await _mediator.Send(new GetBrewCoffeeVmQuery());

            return response.CoffeStatus switch
            {
                CoffeeStatus.Ready => Ok(response.BrewCoffeeInfo),
                CoffeeStatus.ServiceUnavailable => StatusCode(503, "Service Unavailable"),
                CoffeeStatus.Teapot => StatusCode(418, "I’m a teapot"),
                _ => StatusCode(500) // Or any other appropriate status code
            };
          
        }
    }
}
