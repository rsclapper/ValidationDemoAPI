using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.Net;
using ValidationDemoApi.Controllers;

namespace ValidationDemoApi.OrderService.ApiClients
{
    public class ClientService : IClientService
    {
        // HttpFactory injected into the constructor
        private IHttpClientFactory _clientFactory { get; }
        private HttpClient client;
        private CircuitBreakerPolicy circuitBreakerPolicy;
        AsyncTimeoutPolicy<HttpResponseMessage> timeoutPolicy;
        public ClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            HttpClient client = _clientFactory.CreateClient();

            client.BaseAddress = new Uri("https://localhost:5001");
            timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3));
            circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreaker(2, TimeSpan.FromSeconds(5));

        }

        public async Task<List<ContactDto>?> GetContactsAsync()
        {

            var response = await circuitBreakerPolicy.Execute(() => timeoutPolicy.ExecuteAsync(() => client.GetAsync("api/contacts")));

            if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return null;
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                var contacts = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
                return contacts;
            }
            return null;

        }
    }
}
