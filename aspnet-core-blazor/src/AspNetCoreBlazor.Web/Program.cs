using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;
using AspNetCoreBlazor.Web;
using AspNetCoreBlazor.Web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddFluentUIComponents();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(_ => new PlatformDetection(PlatformKind.Browser));
builder.Services.AddSingleton<IUserService, UserService>();

await builder.Build().RunAsync();
