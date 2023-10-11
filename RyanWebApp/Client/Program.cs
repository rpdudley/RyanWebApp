using System.Net.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RyanWebApp.Client;
using RyanWebApp.Client.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");

        // If you need the head outlet functionality (for manipulating the head content), keep this.
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Register the CipherServiceClient.
        builder.Services.AddSingleton<CipherServiceClient>();

        // Register HttpClient for the WebAssembly app with the appropriate base address.
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}
