﻿using AspNetCoreBlazor.Core.Services;
using AspNetCoreBlazor.Core.Types;

namespace AspNetCoreBlazor.Web.Services;

// HACK: WebにはSecureStorageはないため、なにかセキュアな保存方法を検討する必要がある。気が向いたら実装する。
public class UserService : IUserService
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
