namespace AspNetCoreBlazor.Core.Data;

public interface ISecureStorageService
{
  Task<User> GetCurrentUserAsync();
  Task SetCurrentUserAsync(User user);
  void DeleteUser();
}
