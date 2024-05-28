using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NodaTime;
using NodaTime.Testing;
using ReadyTech.BrewMug.Api.Controllers;
using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
using System.Net;

namespace ReadyTech.BrewMug.Api.Test
{
    [TestClass]
    public class BrewControllerTests
    {
        private BrewController _controller;
        private Mock<ILogger<BrewController>> _loggerMock;
        private Mock<IMediator> _mediatorMock;

        [TestInitialize]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<BrewController>>();
            _controller = new BrewController(_mediatorMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task GetBrewCoffeeInfo_With_200StatusCode()
        {
            // Arrange
            var brewCoffeeVm = new BrewCoffeeVm
            {
                 BrewCoffeeInfo=new {
                     Message = "Your piping hot coffee is ready",
                     Prepared = DateTimeOffset.UtcNow
                 },
                 CoffeStatus=AppMgr.Enums.CoffeeStatus.Ready
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetBrewCoffeeVmQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brewCoffeeVm);

            //Act
            var response = await _controller.GetBrewCoffeeAsync() as OkObjectResult;

            // Assert
            Assert.AreEqual(200, response.StatusCode);
            Assert.IsNotNull(response.Value);
        }

        [TestMethod]
        public async Task GetBrewCoffeeInfo_With503Status_OnEvery5thCall()
        {
            // Arrange
            var brewCoffee = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.UtcNow
                },
                CoffeStatus = AppMgr.Enums.CoffeeStatus.Ready
            };

            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new
                {
                    Message = "Service Unavailable"
                },
                CoffeStatus = AppMgr.Enums.CoffeeStatus.ServiceUnavailable
            };

            _mediatorMock
                .SetupSequence(x => x.Send(It.IsAny<GetBrewCoffeeVmQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brewCoffee)
                .ReturnsAsync(brewCoffee)
                .ReturnsAsync(brewCoffee)
                .ReturnsAsync(brewCoffee)
                .ReturnsAsync(brewCoffeeVm);

            for (int i = 0; i < 4; i++)
            {
               await _controller.GetBrewCoffeeAsync();
             }

            var response = await _controller.GetBrewCoffeeAsync() as ObjectResult;

            Assert.AreEqual(503, response.StatusCode);
            Assert.AreEqual("Service Unavailable", (dynamic)response.Value);


        }

        [TestMethod]
        public async Task GetBrewCoffee_With418Status_OnAprilFirst()
        {
            // Arrange
            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new
                {
                    Message = "I’m a teapot"
                },
                CoffeStatus = AppMgr.Enums.CoffeeStatus.Teapot
            };

            _mediatorMock
                .Setup(x => x.Send(It.IsAny<GetBrewCoffeeVmQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(brewCoffeeVm);

            // Act
            var response = await _controller.GetBrewCoffeeAsync() as ObjectResult;

            // Assert
            Assert.AreEqual(418, response.StatusCode);
            Assert.AreEqual("I’m a teapot", (dynamic)response.Value);

        }
    }
}