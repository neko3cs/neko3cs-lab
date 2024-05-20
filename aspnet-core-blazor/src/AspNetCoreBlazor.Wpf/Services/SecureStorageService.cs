using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Wpf.Services;

// HACK: 気が向いたら実装する。WPFならPasswordVaultクラスによる暗号化が良いみたい。
public class SecureStorageService : ISecureStorageService
{
    public Task<User> GetCurrentUserAsync()
    {
        return Task.Run(() => new User("hoge", "fuga"));
    }

    public async Task SetCurrentUserAsync(User user)
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }

    public void DeleteUser()
    {
        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
    }
}
