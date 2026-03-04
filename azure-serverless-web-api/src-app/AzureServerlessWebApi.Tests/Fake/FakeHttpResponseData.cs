using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace AzureServerlessWebApi.Tests.Fake;

/// <summary>
/// テスト用に HTTP レスポンスをシミュレートするための「偽物（Fake）」クラスです。
/// 書き込まれたステータスコードやデータをメモリ上で保持します。
/// </summary>
public class FakeHttpResponseData(FunctionContext functionContext) : HttpResponseData(functionContext)
{
    // 関数の実行結果として返されるステータスコード（200 OK など）
    public override HttpStatusCode StatusCode { get; set; }

    // HTTP ヘッダー（Content-Type など）
    public override HttpHeadersCollection Headers { get; set; } = new HttpHeadersCollection();

    // 実際のレスポンスデータ（JSON 文字列など）を保持するストリーム
    public override Stream Body { get; set; } = new MemoryStream();

    // クッキー情報
    public override HttpCookies Cookies { get; } = new FakeHttpCookies();
}
