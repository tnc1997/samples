using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ConsoleApplication
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            TokenResponse tokenResponse;

            using (var discoveryDocumentHttpClient = new HttpClient())
            {
                var discoveryDocumentResponse = await discoveryDocumentHttpClient.GetDiscoveryDocumentAsync(Environment.GetEnvironmentVariable("AUTHENTICATION__AUTHORITY"));
                Console.WriteLine(discoveryDocumentResponse.TokenEndpoint);

                tokenResponse = await discoveryDocumentHttpClient.RequestClientCredentialsTokenAsync(
                    new ClientCredentialsTokenRequest
                    {
                        Address = discoveryDocumentResponse.TokenEndpoint,
                        ClientId = "console",
                        ClientSecret = "secret",
                        Scope = "api"
                    });
                Console.WriteLine(tokenResponse.AccessToken);
            }

            using (var apiHttpClient = new HttpClient())
            {
                apiHttpClient.SetBearerToken(tokenResponse.AccessToken);

                var response = await apiHttpClient.GetStringAsync($"{Environment.GetEnvironmentVariable("API_URL")}/WeatherForecast");
                Console.WriteLine(response);
            }
        }
    }
}