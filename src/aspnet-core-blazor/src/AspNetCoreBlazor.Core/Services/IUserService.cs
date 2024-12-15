using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Core.Services;

public interface IUserService
{
  Task<User> GetCurrentUserAsync();
  Task SetCurrentUserAsync(User user);
  void DeleteUser();
}
