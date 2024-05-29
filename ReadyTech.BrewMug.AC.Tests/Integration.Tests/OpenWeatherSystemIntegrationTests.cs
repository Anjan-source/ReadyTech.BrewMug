using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using ReadyTech.BrewMug.AppMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.AC.Tests.Integration.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq.Protected;
    using Moq;
    using ReadyTech.BrewMug.AC.OpenWeatherSystem;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Models;
    using ReadyTech.BrewMug.AppMgr.Test.helper;
    using System.Net;

    [TestClass]
    public class OpenWeatherSystemIntegrationTests
    {
        private readonly Mock<ILogger<OpenWeatherSystem>> _loggerMock;

        public OpenWeatherSystemIntegrationTests() 
        {
            _loggerMock = new Mock<ILogger<OpenWeatherSystem>>();
        }

        [TestMethod]
        public async Task GetWeatherData_WithValidResponse()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddHttpClient();
            var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../ReadyTech.BrewMug.Api/"));
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.development.json")
                    .Build();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<IOpenWeatherSystem, OpenWeatherSystem>();
            services.AddTransient<ILogger<OpenWeatherSystem>, Logger<OpenWeatherSystem>>();
            var serviceProvider = services.BuildServiceProvider();

            var openWeatherSystem = new OpenWeatherSystem(serviceProvider.GetService<IConfiguration>(), serviceProvider.GetService<IHttpClientFactory>(), serviceProvider.GetService<ILogger<OpenWeatherSystem>>());

            // Act
            var result = await openWeatherSystem.GetWheatherTemparatureAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<RootObject>(result);
        }


    }
}
