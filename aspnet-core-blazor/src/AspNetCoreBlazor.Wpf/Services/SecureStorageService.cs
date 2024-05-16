using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Wpf.Services;

// TODO: 後で実装する
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
