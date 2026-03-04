using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration;

namespace AzureServerlessWebApi.Middleware;

public class ApiKeyMiddleware(IConfiguration configuration) : IFunctionsWorkerMiddleware
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private readonly string? _expectedApiKey = configuration["WebApiApiKey"];

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpRequestData = await context.GetHttpRequestDataAsync();

        if (httpRequestData != null)
        {
            // ヘッダーからキーを取得
            if (!httpRequestData.Headers.TryGetValues(ApiKeyHeaderName, out var values) || 
                !values.Contains(_expectedApiKey))
            {
                // キーが一致しない場合は 401 Unauthorized を返す
                var response = httpRequestData.CreateResponse(HttpStatusCode.Unauthorized);
                await response.WriteStringAsync("Invalid or missing API Key.");
                context.GetInvocationResult().Value = response;
                return;
            }
        }

        await next(context);
    }
}
