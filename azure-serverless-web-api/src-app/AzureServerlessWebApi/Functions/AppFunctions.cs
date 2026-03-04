using System.Net;
using System.Text.Json;
using AzureServerlessWebApi.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureServerlessWebApi.Functions;

/// <summary>
/// HTTPリクエストを受け取って処理を行うAPI関数クラスです。
/// </summary>
/// <param name="logger">ログ出力用のインスタンス</param>
/// <param name="dbContext">データベース操作用のインスタンス</param>
public class AppFunctions(ILogger<AppFunctions> logger, AppDbContext dbContext)
{
    private readonly ILogger<AppFunctions> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    /// <summary>
    /// データベースのバージョン情報を取得するAPIエンドポイントです。
    /// URL: /api/GetDatabaseVersion
    /// </summary>
    [Function("GetDatabaseVersion")]
    public async Task<HttpResponseData> GetDatabaseVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request: GetDatabaseVersion.");

        try
        {
            // 直接SQLを実行して、SQL Server のバージョン文字列を取得します。
            var results = await _dbContext.Database
                .SqlQueryRaw<string>("SELECT @@VERSION")
                .ToListAsync();

            var version = results.FirstOrDefault();

            // レスポンスを作成して返します。
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(version ?? "Unknown Version");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching database version.");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteStringAsync("Error fetching database version.");
            return response;
        }
    }

    /// <summary>
    /// ユーザー一覧を取得するAPIエンドポイントです。
    /// URL: /api/GetUsers
    /// </summary>
    [Function("GetUsers")]
    public async Task<HttpResponseData> GetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request: GetUsers.");

        try
        {
            // Entity Framework Core を使用して、User テーブルから全件取得します。
            var users = await _dbContext.Users.ToListAsync();

            // 取得結果を JSON 形式に変換して返します。
            var response = req.CreateResponse(HttpStatusCode.OK);
            var json = JsonSerializer.Serialize(users);
            await response.WriteStringAsync(json);

            // HTTP ヘッダーに JSON であることを明示します。
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching users.");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteStringAsync("Error fetching users.");
            return response;
        }
    }
}
