﻿using AspNetCoreBlazor.Core.Data;

namespace AspNetCoreBlazor.Web.Data;

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
