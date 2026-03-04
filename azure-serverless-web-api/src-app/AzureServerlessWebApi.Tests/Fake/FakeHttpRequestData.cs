using System.Collections.Specialized;
using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureServerlessWebApi.Tests.Fake;

/// <summary>
/// テスト用に HTTP リクエストをシミュレートするための「偽物（Fake）」クラスです。
/// 実際の Azure 環境がなくても、リクエストがあったかのように振る舞います。
/// </summary>
public class FakeHttpRequestData(FunctionContext functionContext) : HttpRequestData(functionContext)
{
    public override Stream Body => new MemoryStream();
    public override HttpHeadersCollection Headers => new HttpHeadersCollection();
    public override IReadOnlyCollection<IHttpCookie> Cookies => new List<IHttpCookie>();
    public override Uri Url => new Uri("http://localhost");
    public override NameValueCollection Query => new NameValueCollection();
    public override string Method => "GET";
    public override IEnumerable<ClaimsIdentity> Identities => new List<ClaimsIdentity>();

    /// <summary>
    /// レスポンスオブジェクトを作成する際、本物の代わりに FakeHttpResponseData を返します。
    /// </summary>
    public override HttpResponseData CreateResponse()
    {
        return new FakeHttpResponseData(FunctionContext);
    }
}
