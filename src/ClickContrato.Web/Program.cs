using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ClickContrato.Web;
using ClickContrato.Web.Services.Api;
using ClickContrato.Web.Services.Auth;
using ClickContrato.Web.Services.Storage;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("Api"));
builder.Services.AddScoped(sp =>
{
    var options = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(options.BaseUrl)
        ? "http://localhost:5100"
        : options.BaseUrl.TrimEnd('/');
    return new HttpClient { BaseAddress = new Uri($"{baseUrl}/") };
});

builder.Services.AddScoped<ClickContratoApiClient>();
builder.Services.AddScoped<IKeyValueStorage, BrowserLocalStorage>();
builder.Services.AddScoped<AuthStore>();

await builder.Build().RunAsync();
