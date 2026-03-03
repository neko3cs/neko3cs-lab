using Microsoft.Azure.Functions.Worker.Http;
using Moq;

namespace AzureServerlessWebApi.Tests.Fake;

public class FakeHttpCookies : HttpCookies
{
    public override void Append(string name, string value) { }
    public override void Append(IHttpCookie cookie) { }
    public override IHttpCookie CreateNew() => new Mock<IHttpCookie>().Object;
}
