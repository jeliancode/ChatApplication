using LookMeChatApp.Domain.Model;
using SQLite;

namespace LookMeChatApp.Infraestructure.Repositories;

public class FriendRepository
{
    private readonly SQLiteAsyncConnection _sQLiteDb;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);


    public FriendRepository(SQLiteAsyncConnection sQLiteDb)
    {
        _sQLiteDb = sQLiteDb;
    }

    public async Task AddFriendAsync(Friend friend)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _sQLiteDb.InsertAsync(friend); 
        }
        finally
        {
            _semaphore.Release();
        };
    }

    public async Task<Friend?> FindByIdAsync(Guid id)
    {
        return await _sQLiteDb.Table<Friend>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Friend?> FindByUsernameAsync(string contactName)
    {
        return await _sQLiteDb.Table<Friend>()
            .FirstOrDefaultAsync(x => x.Username == contactName);
    }
}
