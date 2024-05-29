namespace ReadyTech.BrewMug.AppMgr.Test.helper
{
    public class TestHttpClientFactory
    {
        private HttpClient httpClient;

        public TestHttpClientFactory(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}