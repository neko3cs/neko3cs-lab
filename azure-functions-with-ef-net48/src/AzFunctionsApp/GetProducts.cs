using AzFunctionsApp.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
// ReSharper disable ClassNeverInstantiated.Global

namespace AzFunctionsApp
{
    public class GetProducts
    {
        private readonly ILogger _logger;

        public GetProducts(
            ILoggerFactory loggerFactory,
            AdventureWorksLT2016Entities databaseContext
        )
        {
            _logger = loggerFactory.CreateLogger<GetProducts>();
        }

        [Function("GetProducts")]
        public HttpResponseData Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
