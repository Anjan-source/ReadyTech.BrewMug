
namespace ReadyTech.BrewMug.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    ///BaseController
    ///Initialize MediatR instance
    /// </summary>
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
