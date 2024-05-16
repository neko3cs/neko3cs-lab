using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Maui.Services;

public class SecureStorageService : ISecureStorageService
{
    public async Task<User> GetCurrentUserAsync() => new User(
        await SecureStorage.Default.GetAsync("UserId") ?? string.Empty,
        await SecureStorage.Default.GetAsync("Password") ?? string.Empty
      );

    public async Task SetCurrentUserAsync(User user)
    {
        await SecureStorage.Default.SetAsync("UserId", user.UserId);
        await SecureStorage.Default.SetAsync("Password", user.Password);
    }

    public void DeleteUser()
    {
        SecureStorage.Default.Remove("UserId");
        SecureStorage.Default.Remove("Password");
    }
}
