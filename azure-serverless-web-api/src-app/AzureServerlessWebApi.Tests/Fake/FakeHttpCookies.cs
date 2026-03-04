using Microsoft.Azure.Functions.Worker.Http;
using Moq;

namespace AzureServerlessWebApi.Tests.Fake;

/// <summary>
/// テスト用に HTTP クッキーをシミュレートするための「偽物（Fake）」クラスです。
/// </summary>
public class FakeHttpCookies : HttpCookies
{
    // クッキーを追加する操作を無視するように設定します（テストで必要ないため）。
    public override void Append(string name, string value) { }
    public override void Append(IHttpCookie cookie) { }

    // 新しいクッキーオブジェクトを作成する際、空のモックを返します。
    public override IHttpCookie CreateNew() => new Mock<IHttpCookie>().Object;
}
