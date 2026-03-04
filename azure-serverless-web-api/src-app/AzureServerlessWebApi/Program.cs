using AzureServerlessWebApi.Data;
using AzureServerlessWebApi.Middleware;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// アプリケーションの起動設定を行うビルダーを作成します。
var builder = FunctionsApplication.CreateBuilder(args);

// APIキー認証を行うための独自ミドルウェアを登録します。
// これにより、すべてのAPIリクエストで「X-API-KEY」ヘッダーのチェックが行われるようになります。
builder.UseMiddleware<ApiKeyMiddleware>();

// データベース接続文字列を環境変数「DbConnectionString」から取得します。
var connectionString = builder.Configuration["DbConnectionString"];

// データベース（Entity Framework Core）の設定を行います。
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    // 接続文字列がある場合は SQL Server を使用し、ない場合は開発用のメモリ内 DB を使用します。
    if (!string.IsNullOrEmpty(connectionString))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseInMemoryDatabase("DevDb");
    }
});

// アプリケーションの動作状況を監視するための Application Insights を設定します。
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// アプリケーション（ホスト）をビルドします。
var host = builder.Build();

// 【Code First】データベースの自動作成と初期データの投入処理
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // データベースが存在しない場合は新しく作成します（テーブル構造も自動適用されます）。
    var isCreated = await dbContext.Database.EnsureCreatedAsync();

    // 初回作成時、またはユーザーが一人もいない場合にサンプルデータを5件投入します。
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

// アプリケーションを実行し、リクエストの待機を開始します。
host.Run();
