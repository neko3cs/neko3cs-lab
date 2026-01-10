using AzFunctionsApp.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net;
using JsonSerializer = System.Text.Json.JsonSerializer;

// ReSharper disable ClassNeverInstantiated.Global

namespace AzFunctionsApp
{
    public class GetProducts
    {
        private readonly AzFunctionsAppDatabaseContext _dbContext;
        private readonly ILogger _logger;

        public GetProducts(
            ILoggerFactory loggerFactory,
            AzFunctionsAppDatabaseContext dbContext
        )
        {
            _dbContext = dbContext;
            _logger = loggerFactory.CreateLogger<GetProducts>();
        }

        [Function("GetProducts")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product/{id:int}")]
            HttpRequestData req,
            int id
        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (id < 0)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            };

            var products = _dbContext
                .Product
                .Where(x => x.ProductID == id)
                .ToList();
            var json = JsonSerializer.Serialize(products);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");

            response.WriteString(json);

            return response;
        }
    }
}
