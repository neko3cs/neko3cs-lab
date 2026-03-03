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

// Code First: データベースの自動作成 & Seed データの投入
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    // データベースが存在しない場合は作成し、スキーマを適用する
    var isCreated = await dbContext.Database.EnsureCreatedAsync();
    
    // データが空の場合のみ、サンプルデータを投入
    if (isCreated || !await dbContext.Users.AnyAsync())
    {
        dbContext.Users.AddRange(new List<User>
        {
            new() { Name = "田中 太郎", Email = "tanaka@example.com" },
            new() { Name = "佐藤 次郎", Email = "sato@example.com" },
            new() { Name = "鈴木 三郎", Email = "suzuki@example.com" },
            new() { Name = "高橋 四郎", Email = "takahashi@example.com" },
            new() { Name = "伊藤 五郎", Email = "ito@example.com" }
        });
        await dbContext.SaveChangesAsync();
    }
}

host.Run();
