using System.Net;
using System.Text.Json;
using AzureServerlessWebApi.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AzureServerlessWebApi;

public class AppFunctions(ILogger<AppFunctions> logger, AppDbContext dbContext)
{
    private readonly ILogger<AppFunctions> _logger = logger;
    private readonly AppDbContext _dbContext = dbContext;

    [Function("GetDatabaseVersion")]
    public async Task<HttpResponseData> GetDatabaseVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request: GetDatabaseVersion.");

        try
        {
            var results = await _dbContext.Database
                .SqlQueryRaw<string>("SELECT @@VERSION")
                .ToListAsync();

            var version = results.FirstOrDefault();

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

    [Function("GetUsers")]
    public async Task<HttpResponseData> GetUsers(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request: GetUsers.");

        try
        {
            var users = await _dbContext.Users.ToListAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            var json = JsonSerializer.Serialize(users);
            await response.WriteStringAsync(json);
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
