
namespace ReadyTech.BrewMug.AppMgr.Test.queries
{
    using Microsoft.Extensions.Logging;
    using Moq;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
    using ReadyTech.BrewMug.AppMgr.Models;

    [TestClass]
    public class GetBrewCoffeeQueryHandlerTest
    {
        private readonly Mock<IBrewService> _brewService;
        private readonly Mock<ILogger<GetBrewCoffeeVmQueryHandler>> _logger;
        private readonly Mock<IOpenWeatherSystem> _openWeatherSystem;
        public GetBrewCoffeeQueryHandlerTest()
        {
            _logger = new Mock<ILogger<GetBrewCoffeeVmQueryHandler>>();
            _brewService = new Mock<IBrewService>();
            _brewService = new Mock<IBrewService>();
            _openWeatherSystem = new Mock<IOpenWeatherSystem>();


        }

        [TestMethod]
        public async Task GetBrewCoffeeInfoHandler_TestWeatherAPI_MoreThan30cTemparature()
        {
            var handler = new GetBrewCoffeeVmQueryHandler(_openWeatherSystem.Object, _brewService.Object, _logger.Object);

            var mockWeatherResponse = new RootObject { main = new Main { temp = 304.15 } }; // temperature in Kelvin
            var mockBrewCoffeeVm = new BrewCoffeeVm { BrewCoffeeInfo = new BrewCoffee() };

            _openWeatherSystem.Setup(x => x.GetWheatherTemparatureAsync()).ReturnsAsync(mockWeatherResponse);
            _brewService.Setup(x => x.GetBrewCoffeeInformationAsync()).ReturnsAsync(mockBrewCoffeeVm);

            // Act
            var result = await handler.Handle(new GetBrewCoffeeVmQuery(), new CancellationToken());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Your refreshing iced coffee is ready", result.BrewCoffeeInfo.Message);

        }

        public async Task Get_BrewCoffeeInfoHandler_TestWeatherAPI_LessThan30cTemparature()
        {
            var handler = new GetBrewCoffeeVmQueryHandler(_openWeatherSystem.Object, _brewService.Object, _logger.Object);

            var mockWeatherResponse = new RootObject { main = new Main { temp = 302.15 } }; // temperature in Kelvin
            var mockBrewCoffeeVm = new BrewCoffeeVm { BrewCoffeeInfo = new BrewCoffee() };

            _openWeatherSystem.Setup(x => x.GetWheatherTemparatureAsync()).ReturnsAsync(mockWeatherResponse);
            _brewService.Setup(x => x.GetBrewCoffeeInformationAsync()).ReturnsAsync(mockBrewCoffeeVm);

            // Act
            var result = await handler.Handle(new GetBrewCoffeeVmQuery(), new CancellationToken());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual("Your refreshing iced coffee is ready", result.BrewCoffeeInfo.Message);

        }

        [TestMethod]
        public async Task Get_BrewCoffeeInfo_Handler_Test_WhenException()
        {
            //Arrange
            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new BrewCoffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.UtcNow
                },
                CoffeStatus = AppMgr.Enums.CoffeeStatus.Teapot
            };

            var handler = new GetBrewCoffeeVmQueryHandler(_openWeatherSystem.Object, _brewService.Object, _logger.Object);

            _openWeatherSystem.Setup(x => x.GetWheatherTemparatureAsync()).Throws(new Exception("Exception"));

            // Act
            var result = await handler.Handle(new GetBrewCoffeeVmQuery(), new CancellationToken());

            // Assert
            Assert.IsNull(result);
            Assert.AreNotEqual(brewCoffeeVm, result);
        }


    }
}
