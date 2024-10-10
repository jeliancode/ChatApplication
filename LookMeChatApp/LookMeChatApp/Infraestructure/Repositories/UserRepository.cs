using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Model;
using SQLite;

namespace LookMeChatApp.Infraestructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly SQLiteAsyncConnection _sQLiteDb;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


    public UserRepository(SQLiteAsyncConnection sQLiteDb)
    {
        _sQLiteDb = sQLiteDb;
    }

    public async Task AddUserAsync(User user)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _sQLiteDb.InsertAsync(user);
        }
        finally
        {
            _semaphore.Release();
        };
    }

    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await _sQLiteDb.Table<User>()
            .FirstOrDefaultAsync(x => x.IdUser == id);
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await _sQLiteDb.Table<User>()
            .FirstOrDefaultAsync(x => x.Username == username);
    }
}
