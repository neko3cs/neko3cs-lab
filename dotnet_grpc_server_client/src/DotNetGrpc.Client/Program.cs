using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DotNetGrpc.Client;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using DotNetGrpc.Shared;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(services => 
{
    var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
    // HTTPS証明書エラーを回避するため、HTTPを使用します。
    // 開発環境では localhost の自己署名証明書が信頼されていないと Failed to fetch になるためです。
    var channel = GrpcChannel.ForAddress("http://localhost:5002", new GrpcChannelOptions { HttpClient = httpClient });
    return new Greeter.GreeterClient(channel);
});

await builder.Build().RunAsync();
