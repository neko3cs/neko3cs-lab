using AspNetCoreBlazor.Core.Data;
using AspNetCoreBlazor.Web;
using AspNetCoreBlazor.Web.Data;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(_ => new PlatformDetection(PlatformKind.Browser));
builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();

await builder.Build().RunAsync();
