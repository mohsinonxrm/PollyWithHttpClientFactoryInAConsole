using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PollyWithHttpClientFactoryInAConsole
{
    public class BusinessService : IHostedService
    {
        private IHttpClientFactory _httpClientFactory;
            public BusinessService(IHttpClientFactory httpClientFactory)
            {
                _httpClientFactory = httpClientFactory;
            }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await MakeRequestsToRemoteService();
        }

        public async Task MakeRequestsToRemoteService()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("JsonplaceholderClient");
            var response = await httpClient.GetAsync("/photos/1");
            Photo photo = await response.Content.ReadAsAsync<Photo>();
            Console.WriteLine(photo);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
