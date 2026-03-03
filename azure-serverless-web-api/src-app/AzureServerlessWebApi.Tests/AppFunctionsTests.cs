using System.Net;
using AzureServerlessWebApi.Data;
using AzureServerlessWebApi.Tests.Fake;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace AzureServerlessWebApi.Tests;

public class AppFunctionsTests
{
    private readonly Mock<ILogger<AppFunctions>> _loggerMock = new();
    private readonly AppDbContext _dbContext;

    public AppFunctionsTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new AppDbContext(options);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUsersFromFunction()
    {
        // Arrange
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var functions = new AppFunctions(_loggerMock.Object, _dbContext);
        var contextMock = new Mock<FunctionContext>();
        var request = new FakeHttpRequestData(contextMock.Object);

        // Act
        var response = await functions.GetUsers(request);

        // Assert
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        
        response.Body.Position = 0;
        using var reader = new StreamReader(response.Body);
        var responseBody = await reader.ReadToEndAsync();
        responseBody.ShouldContain("Test User");
    }

    [Fact]
    public async Task GetDatabaseVersion_ShouldHandleError_WhenRelationalNotSupported()
    {
        // Arrange
        var functions = new AppFunctions(_loggerMock.Object, _dbContext);
        var contextMock = new Mock<FunctionContext>();
        var request = new FakeHttpRequestData(contextMock.Object);

        // Act & Assert
        // InMemory プロバイダーでは SqlQueryRaw が例外を投げるため、InternalServerError になることを確認
        var response = await functions.GetDatabaseVersion(request);
        
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }
}
