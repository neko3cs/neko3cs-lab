using AspNetCoreBlazor.Core.Data;

namespace AspNetCoreBlazor.Maui;

public class SecureStorageService : ISecureStorageService
{
  public async Task<User> GetCurrentUserAsync() => new User(
      await SecureStorage.Default.GetAsync("UserId"),
      await SecureStorage.Default.GetAsync("Password")
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
