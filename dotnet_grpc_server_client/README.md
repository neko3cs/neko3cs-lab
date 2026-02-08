# .NET gRPC-Web サンプルプロジェクト (Blazor WASM & ASP.NET Core)

## はじめに

このプロジェクトは、.NETを使用してgRPCサーバーとBlazor WebAssembly (WASM) クライアントを構築し、gRPC-Webプロトコルを用いて通信を行うサンプルです。

## 重要：このプロジェクトで発生すること

**現在、このプロジェクトはクライアントからサーバーへの通信時に CORS エラーが発生し、正常に動作しません。**

### エラー内容

ブラウザの開発者ツール（コンソール）にて以下のエラーが出力されます。
`Access to fetch at 'http://localhost:5002/greet.Greeter/SayHello' from origin 'http://localhost:5270' has been blocked by CORS policy: Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' header is present on the requested resource.`

### 原因と解決について

- **原因**: ブラウザが送るPreflight (OPTIONS) リクエストに対して、サーバー（ASP.NET Core）が適切なCORSヘッダーを返せていない、あるいはgRPC-Webミドルウェアとの競合によりPreflight理が正しく行われていない可能性があります。
- **試行した解決策**: CORSドルウェアの順序（UseRouting, UseCors, UseGrpcWeb）の変更、`AllowAnyOrigin` や `RequireCors` の適用、`GrpcWebOptions` の設定変更などを試みましたが、環境固有の問題（ポート競合やブラウザの挙動等）を含め、解決に至りませんでした。

## gRPCとは

gRPC (gRPC Remote Procedure Calls) は、Googleによって開発されたオープンソースの高性能PCフレームワークです。

- **Protocol Buffers (protobuf)** を使用してインターフェイスを定義します。
- HTTP/2をベースとしており、双方向ストリーミングやバイナリ転送による高速な通信が可能です。

## gRPCを.NETで使用するには

.NETでは、以下のパッケージを使用してgRPCを実装します。

- **サーバーサイド**: `Grpc.AspNetCore`
- **クライアントサイド**: `Grpc.Net.Client`
- **gRPC-Web対応**: `Grpc.AspNetCore.Web` (サーバー), `Grpc.Net.Client.Web` (クライアント）

## protoファイルの書き方

`src/DotNetGrpc.Proto/greet.proto` のように、サービス名、メソッド、リクエスト・レスポンスのメッセージ型を定義します。

```proto
syntax = "proto3";
option csharp_namespace = "DotNetGrpc.Shared";
package greet;

service Greeter {
  rpc SayHello (HelloRequest) returns (HelloReply);
}

message HelloRequest { string name = 1; }
message HelloReply { string message = 1; }
```

## protoファイルからクラスをビルドする方法

`.csproj` ファイルに `Protobuf` 項目を追加します。これによりビルド時に自動的にC# クラスが生成されます。

**サーバー (`DotNetGrpc.Server.csproj`)**

```xml
<ItemGroup>
  <Protobuf Include="..\DotNetGrpc.Proto\greet.proto" GrpcServices="Server" Link="Protos\greet.proto" />
</ItemGroup>
```

**クライアント (`DotNetGrpc.Client.csproj`)**

```xml
<ItemGroup>
  <Protobuf Include="..\DotNetGrpc.Proto\greet.proto" GrpcServices="Client" Link="Protos\greet.proto" />
</ItemGroup>
```

## サーバーサイドの設定・実装方法

サーバーサイド（ASP.NET Core）では、gRPCサービスの定義と、ブラウザからの通信を許可するためのgRPC-WebおよびCORSの設定が必要です。

### 1. サービスの追加と CORS ポリシーの定義

`Program.cs` でgRPCサービスを有効にし、クライアント（Blazor WASM）からのクロスドメインリクエストを許可する設定を行います。

```csharp
// src/DotNetGrpc.Server/Program.cs
var builder = WebApplication.CreateBuilder(args);

// gRPCサービスの登録
builder.Services.AddGrpc();

// CORSポリシーの定義: gRPC-Web特有のヘッダーを公開する必要がある
builder.Services.AddCors(o => o.AddPolicy("AllowAll", policy =>
{
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
          .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));
```

### 2. ミドルウェアパイプラインの設定

gRPC-WebとCORSを正しく動作させるためには、ミドルウェアの順序が重要です。

```csharp
// src/DotNetGrpc.Server/Program.cs
var app = builder.Build();

app.UseRouting();

// 1. gRPC-Webを有効化
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

// 2. CORSを適用
app.UseCors("AllowAll");

// 3. サービスのマッピングと設定
app.MapGrpcService<GreeterService>().EnableGrpcWeb().RequireCors("AllowAll");
```

### 3. サービスのロジック実装

`.proto` ファイルから自動生成された基底クラスを継承して実装します。

```csharp
// src/DotNetGrpc.Server/Services/GreeterService.cs
using Grpc.Core;
using DotNetGrpc.Shared;

namespace DotNetGrpc.Server.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}!"
        });
    }
}
```

### 必要な NuGet パッケージ

- `Grpc.AspNetCore`
- `Grpc.AspNetCore.Web`

## クライアントサイドの設定・実装方法

Blazor WASMではブラウザの制約により標準のgRPC (HTTP/2) が使えないため、`GrpcWebHandler` を使用します。

```csharp
var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
var channel = GrpcChannel.ForAddress("http://localhost:5002", new GrpcChannelOptions { HttpClient = httpClient });
var client = new Greeter.GreeterClient(channel);
```

## このプロジェクトの実行方法

以下のコマンドをそれぞれのプロジェクトディレクトリ、またはルートから実行します。

### 1. サーバーの起動

```bash
dotnet run --project src/DotNetGrpc.Server/DotNetGrpc.Server.csproj --launch-profile http
```

(<http://localhost:5002> で待機）

### 2. クライアントの起動

```bash
dotnet run --project src/DotNetGrpc.Client/DotNetGrpc.Client.csproj --launch-profile http
```

(<http://localhost:5270> で起動）

ブラウザで `http://localhost:5270` にアクセスしてください。
