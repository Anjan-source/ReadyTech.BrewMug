

namespace ReadyTech.BrewMug.AC.Tests.Functional.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Moq.Protected;
    using ReadyTech.BrewMug.AC.OpenWeatherSystem;
    using ReadyTech.BrewMug.AppMgr.Interfaces;
    using ReadyTech.BrewMug.AppMgr.Models;
    using ReadyTech.BrewMug.AppMgr.Test.helper;
    using System.Net;
    using System.Text;

    [TestClass]
    public class OpenWeatherSystemTests
    {
        private readonly Mock<ILogger<OpenWeatherSystem>> _loggerMock;
        public OpenWeatherSystemTests() 
        {
            _loggerMock = new Mock<ILogger<OpenWeatherSystem>>();
        }
        [TestMethod]
        public async Task GetWeatherData_WithValidResponse()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> {
                    {"ExternalServices:WeatherApiUrl", "http://api.openweathermap.org/"},
                    {"ExternalServices:City", "Auckland"},
                    {"ExternalServices:ApiKey", "12345"},
                };
            IConfiguration configuration = new ConfigurationBuilder()
                        .AddInMemoryCollection(inMemorySettings)
                        .Build();

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

            // Setup the SendAsync method to return a HttpResponseMessage with your test weather data
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"main\":{\"temp\":303.15}}", Encoding.UTF8, "application/json")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            var httpClientFactory = new TestHttpClientFactory(httpClient);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            // Setup the CreateClient method to return your mock HttpClient
            mockHttpClientFactory.Setup(_ => _.CreateClient("OpenWeatherApi")).Returns(httpClient);

            var openWeatherSystem = new OpenWeatherSystem(configuration, mockHttpClientFactory.Object, _loggerMock.Object);

            // Act
            var result = await openWeatherSystem.GetWheatherTemparatureAsync();

            // Assert
            // Verify that the result is as expected
            Assert.IsNotNull(result);
            Assert.AreEqual(303.15, result.main.temp);
        }

    }
}