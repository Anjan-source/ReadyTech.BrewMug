namespace ReadyTech.BrewMug.AC.common
{
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Polly;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Target API manages the external calls from application layer
    /// Also handles retry logic with circuit breaker pattren
    /// </summary>
    public abstract class TargetApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private readonly AsyncPolicy _retryPolicy;
        private readonly ILogger<TargetApi> _logger;

        protected TargetApi(IHttpClientFactory httpClientFactory, ILogger<TargetApi> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

            _retryPolicy = Policy
                            .Handle<HttpRequestException>()
                            .WaitAndRetryAsync(3,
                                 retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                 (exception, span, retryAttempt, content) =>
                                 {
                                     if (retryAttempt == 1)
                                     {
                                         logger.LogError($"Exception during first retry: {exception.Message}");
                                     }
                                     logger.LogError($"RETRY: {retryAttempt}, SLEEP: {span.TotalMilliseconds}");
                                 });


        }
        /// <summary>
        /// Sends an HTTP GET request to the specified <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="TResponseContent">
        /// The type of response content to return.
        /// </typeparam>
        /// <param name="path">
        /// Required URL path to send the request to.
        /// </param>
        protected Task<TResponseContent> GetAsync<TResponseContent>(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            return GetInternalAsync<TResponseContent>(path);
        }

        private async Task<TResponseContent> GetInternalAsync<TResponseContent>(string path)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                TResponseContent response =
                    await ExecuteHttpCallAsync<TResponseContent>(path)
                        .ConfigureAwait(false);
                return response;
            }).ConfigureAwait(false);
        }

        private async Task<TResponseContent> ExecuteHttpCallAsync<TResponseContent>(string path)
        {
            try
            {
                //Create Client names in a constatnt file and use it here 
                var httpClient = _httpClientFactory.CreateClient("OpenWeatherApi");
                var request = new HttpRequestMessage(HttpMethod.Get, path);
                //here you can add header that are required to make a call weather API (key, city etc.)
                //also we can create a common GenerateRequestmessage() method and it can be used for other calls as well 

                HttpResponseMessage result = await httpClient.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();

                    var response = JsonConvert.DeserializeObject<TResponseContent>(responseContent);

                    return response;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError("Error deserializing JSON: " + ex.Message);

            }
            return default;
        }

    }
}
