using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Core.Services;

public interface ISecureStorageService
{
  Task<User> GetCurrentUserAsync();
  Task SetCurrentUserAsync(User user);
  void DeleteUser();
}
