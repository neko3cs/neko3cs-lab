using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using AzureServerlessWebApi.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// Key Vault の設定
var keyVaultUrl = Environment.GetEnvironmentVariable("KEY_VAULT_URL");
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    var secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
    builder.Configuration.AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions());
}

// DbContext の登録
var connectionString = builder.Configuration["DbConnectionString"];

builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("DevDb");
    }
});

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

var host = builder.Build();

// Code First: データベースの自動作成 (Migration を使わないシンプルな初期化)
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // データベースが存在しない場合は作成し、スキーマを適用する
    await dbContext.Database.EnsureCreatedAsync();
}

host.Run();
