using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Quartz;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using ReadyTech.BrewMug.AppMgr.queries.BrewCoffee;
using ReadyTech.BrewMug.Data.Interfaces;
using ReadyTech.BrewMug.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ReadyTech.BrewMug.Domain.Services.BrewService;

namespace ReadyTech.BrewMug.Domain.Test.Services
{
    [TestClass]
    public class BrewServiceTests
    {
        private readonly Mock<ILogger<BrewService>> _logger;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IBrewRepository> _brewRepository;

        public BrewServiceTests()
        {
            _logger = new Mock<ILogger<BrewService>>();
            _mapper = new Mock<IMapper>();
            _brewRepository = new Mock<IBrewRepository>();
        }

        [TestMethod]
        public async Task GetBrewCoffeeInfo_When1st_OfApril()
        {
            //Arrange
            _utcTime.Now = () => new DateTime(2024, 4, 1);
            var brewService = new BrewService(_brewRepository.Object, _mapper.Object, _logger.Object);

            // Act
            var result = await brewService.GetBrewCoffeeInformationAsync();
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(CoffeeStatus.Teapot, (CoffeeStatus)result.CoffeStatus);
        }

        [TestMethod]
        public async Task GetBrewCoffeeInfo_WhenRequestCountIs5()
        {
            //Arrange
            _utcTime.Now = () => DateTimeOffset.UtcNow;
            var brewService = new BrewService(_brewRepository.Object, _mapper.Object, _logger.Object);

            for (int i = 0; i < 4; i++)
            {
                await brewService.GetBrewCoffeeInformationAsync();
            }

            // Act
            var result = await brewService.GetBrewCoffeeInformationAsync();
            var resultAfterReset = await brewService.GetBrewCoffeeInformationAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(CoffeeStatus.ServiceUnavailable, (CoffeeStatus)result.CoffeStatus);
            Assert.AreEqual("Your piping hot coffee is ready",result.BrewCoffeeInfo.Message);
            Assert.AreEqual(CoffeeStatus.Ready, (CoffeeStatus)resultAfterReset.CoffeStatus);
        }

        public async Task GetBrewCoffeeInfo_WhenRequestCountLessThan5()
        {
            //Arrange
            _utcTime.Now = () => DateTimeOffset.UtcNow;
            var brewService = new BrewService(_brewRepository.Object, _mapper.Object, _logger.Object);

            for (int i = 0; i < 4; i++)
            {
                await brewService.GetBrewCoffeeInformationAsync();
            }

            // Act
            var result = await brewService.GetBrewCoffeeInformationAsync();
            var resultAfterReset = await brewService.GetBrewCoffeeInformationAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(CoffeeStatus.Ready, (CoffeeStatus)result.CoffeStatus);
            Assert.AreEqual("Your piping hot coffee is ready", result.BrewCoffeeInfo.Message);
        }
    }
}
