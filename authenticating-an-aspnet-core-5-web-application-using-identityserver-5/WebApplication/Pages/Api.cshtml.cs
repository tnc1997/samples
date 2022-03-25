using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Pages
{
    [Authorize]
    public class ApiModel : PageModel
    {
        public ApiModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            Configuration = configuration;
            HttpClientFactory = httpClientFactory;
        }

        public string WeatherForecast { get; set; }

        private IConfiguration Configuration { get; }
        
        private IHttpClientFactory HttpClientFactory { get; }

        public async Task OnGetAsync()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            using (var httpClient = HttpClientFactory.CreateClient())
            {
                httpClient.SetBearerToken(accessToken);

                WeatherForecast = await httpClient.GetStringAsync($"{Configuration["ApiUrl"]}/WeatherForecast");
            }
        }
    }
}