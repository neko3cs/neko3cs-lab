using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;

namespace AzureServerlessWebApi.Middleware;

/// <summary>
/// 全てのHTTPリクエストに対してAPIキーのチェックを行うミドルウェアです。
/// </summary>
/// <param name="configuration">設定情報（環境変数など）にアクセスするためのインスタンス</param>
public class ApiKeyMiddleware(IConfiguration configuration) : IFunctionsWorkerMiddleware
{
    private const string ApiKeyHeaderName = "X-API-KEY";

    // 設定情報（App Settings / 環境変数）から期待されるAPIキーを取得します。
    private readonly string? _expectedApiKey = configuration["WebApiApiKey"];

    /// <summary>
    /// ミドルウェアのメイン処理です。
    /// </summary>
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        // リクエスト情報を取得します。
        var httpRequestData = await context.GetHttpRequestDataAsync();

        if (httpRequestData != null)
        {
            // HTTP ヘッダーから「X-API-KEY」の値を取り出します。
            // キーが含まれていない、または設定値と一致しない場合はエラーを返します。
            if (!httpRequestData.Headers.TryGetValues(ApiKeyHeaderName, out var values) ||
                !values.Contains(_expectedApiKey))
            {
                // 401 Unauthorized（認証失敗）レスポンスを作成します。
                var response = httpRequestData.CreateResponse(HttpStatusCode.Unauthorized);
                await response.WriteStringAsync("Invalid or missing API Key.");

                // 後続の処理（Function 本体など）を実行せずにここで終了します。
                context.GetInvocationResult().Value = response;
                return;
            }
        }

        // 認証に成功した場合は、次の処理（他のミドルウェアやFunction本体）へ進みます。
        await next(context);
    }
}
