using System.Net;
using AzureServerlessWebApi.Data;
using AzureServerlessWebApi.Functions;
using AzureServerlessWebApi.Tests.Fake;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace AzureServerlessWebApi.Tests.Functions;

/// <summary>
/// API 関数の動作を検証するテストクラスです。
/// </summary>
public class AppFunctionsTests
{
    private readonly Mock<ILogger<AppFunctions>> _loggerMock = new();
    private readonly AppDbContext _dbContext;

    public AppFunctionsTests()
    {
        // テストごとに独立したデータベース（メモリ内）を作成します。
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
    }

    /// <summary>
    /// GetUsers 関数がデータベースから正しくユーザーを取得して返すことをテストします。
    /// </summary>
    [Fact]
    public async Task GetUsers_ShouldReturnUsersFromFunction()
    {
        // --- 1. 準備 (Arrange) ---
        // テスト用のデータをメモリ内のデータベースに保存します。
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        // テスト対象の関数クラスを作成します。
        var functions = new AppFunctions(_loggerMock.Object, _dbContext);

        // Azure Functions の仕組みをシミュレートするためのモック（偽物）を作成します。
        var contextMock = new Mock<FunctionContext>();
        var request = new FakeHttpRequestData(contextMock.Object);

        // --- 2. 実行 (Act) ---
        // 実際に関数を呼び出します。
        var response = await functions.GetUsers(request);

        // --- 3. 検証 (Assert) ---
        // レスポンスが null ではないこと、ステータスが OK (200) であることを確認します。
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        // レスポンスの内容（Body）を読み取り、テスト用データが含まれているか確認します。
        response.Body.Position = 0;
        using var reader = new StreamReader(response.Body);
        var responseBody = await reader.ReadToEndAsync();
        responseBody.ShouldContain("Test User");
    }

    /// <summary>
    /// GetDatabaseVersion 関数が、リレーショナルDBではない（InMemory）環境で
    /// 適切にエラー（500）を返すことをテストします。
    /// </summary>
    [Fact]
    public async Task GetDatabaseVersion_ShouldHandleError_WhenRelationalNotSupported()
    {
        // --- 1. 準備 (Arrange) ---
        var functions = new AppFunctions(_loggerMock.Object, _dbContext);
        var contextMock = new Mock<FunctionContext>();
        var request = new FakeHttpRequestData(contextMock.Object);

        // --- 2. 実行 (Act) ---
        // InMemory プロバイダーでは SqlQueryRaw (@@VERSION) が例外を投げるため、関数の try-catch が走ります。
        var response = await functions.GetDatabaseVersion(request);

        // --- 3. 検証 (Assert) ---
        // 関数内部でエラーがキャッチされ、InternalServerError (500) が返ることを確認します。
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);
    }
}
