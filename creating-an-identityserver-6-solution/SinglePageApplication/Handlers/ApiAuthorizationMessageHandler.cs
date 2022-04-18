using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace SinglePageApplication.Handlers;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation) : base(provider,
        navigation)
    {
        ConfigureHandler(new List<string> { "https://api:7001" }, new List<string> { "https://www.example.com/api" });
    }
}
