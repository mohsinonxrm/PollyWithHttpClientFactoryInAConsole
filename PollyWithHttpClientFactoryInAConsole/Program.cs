using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Registry;

namespace PollyWithHttpClientFactoryInAConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    IPolicyRegistry<string> registry = services.AddPolicyRegistry();

                    IAsyncPolicy<HttpResponseMessage> httWaitAndpRetryPolicy =
                        Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

                    registry.Add("SimpleWaitAndRetryPolicy", httWaitAndpRetryPolicy);

                    IAsyncPolicy<HttpResponseMessage> noOpPolicy = Policy.NoOpAsync()
                        .AsAsyncPolicy<HttpResponseMessage>();

                    registry.Add("NoOpPolicy", noOpPolicy);

                    services.AddHttpClient("JsonplaceholderClient", client =>
                    {
                        client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    }).AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
                    {
                        if (httpRequestMessage.Method == HttpMethod.Get || httpRequestMessage.Method == HttpMethod.Delete)
                        {
                            return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleWaitAndRetryPolicy");
                        }
                        return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("NoOpPolicy");
                    });
                        
                    services.AddSingleton<IHostedService, BusinessService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
