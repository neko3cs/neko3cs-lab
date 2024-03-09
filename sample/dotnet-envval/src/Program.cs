var value = Environment.GetEnvironmentVariable("DotnetEnvVal.Test", EnvironmentVariableTarget.User);
if (string.IsNullOrEmpty(value))
{
    Environment.SetEnvironmentVariable("DotnetEnvVal.Test", "test value", EnvironmentVariableTarget.User);
    Console.WriteLine("環境変数を設定しました。");
}
else
{
    Console.WriteLine($"環境変数を取得しました。: {value}");
}