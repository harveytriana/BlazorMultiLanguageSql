// --------------------------------
// blazorspread.net
// --------------------------------
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorMultiLanguage
{
public class Program
{
    const string API_ROOT = "https://localhost:5001";

    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        // The server must allow the client url in its CORS configuration
        builder.Services.AddHttpClient("AspNetApi", _ => _.BaseAddress = new Uri(API_ROOT));

        builder.Services.AddSingleton<LangService>();

        // setup application language
        var host = builder.Build();
        var langService = host.Services.GetService<LangService>();
        await langService.LoadLanguageAsync();

        await builder.Build().RunAsync();
    }
}
}
