using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Web.Services;

// HACK: ページ遷移時先にDLL読んでるのか実装がないとエラーしてしまうので一旦用意 -> 実装しなくても回避する方法があればそちらにする
public class SecureStorageService : ISecureStorageService
{
    public Task<User> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }

    public Task SetCurrentUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser()
    {
        throw new NotImplementedException();
    }
}
