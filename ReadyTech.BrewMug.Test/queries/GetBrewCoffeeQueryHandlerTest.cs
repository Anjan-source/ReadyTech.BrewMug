using Castle.Core.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.AppMgr.Test.queries
{
    [TestClass]
    public class GetBrewCoffeeQueryHandlerTest
    {
        private readonly Mock<IBrewService> _brewService;
        private readonly Mock<ILogger<GetBrewCoffeeVmQueryHandler>> _logger;

        public GetBrewCoffeeQueryHandlerTest()
        {
            _logger = new Mock<ILogger<GetBrewCoffeeVmQueryHandler>>();
            _brewService = new Mock<IBrewService>(); ;
        }

        [TestMethod]
        public async Task Get_BrewCoffeeInfo_Handler_Test_ForValidResponse()
        {
            //Arrange
            var brewCoffeeVm = new BrewCoffeeVm
            {
                BrewCoffeeInfo = new BrewCoffee
                {
                    Message = "Your piping hot coffee is ready",
                    Prepared = DateTimeOffset.UtcNow
                },
                CoffeStatus = AppMgr.Enums.CoffeeStatus.Ready
            };

            var handler = new GetBrewCoffeeVmQueryHandler(_brewService.Object, _logger.Object);

            _brewService.Setup(x => x.GetBrewCoffeeInformationAsync()).ReturnsAsync(brewCoffeeVm);

            // Act
            var result = await handler.Handle(new GetBrewCoffeeVmQuery(), new CancellationToken());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(brewCoffeeVm, result);
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

            var handler = new GetBrewCoffeeVmQueryHandler(_brewService.Object, _logger.Object);

            _brewService.Setup(x => x.GetBrewCoffeeInformationAsync()).Throws(new Exception("Exception"));

            // Act
            var result = await handler.Handle(new GetBrewCoffeeVmQuery(), new CancellationToken());

            // Assert
            Assert.IsNull(result);
            Assert.AreNotEqual(brewCoffeeVm, result);
        }


    }
}
