using LookMeChatApp.Domain.Model;
using SQLite;

namespace LookMeChatApp.Infraestructure.Repositories;

public class FriendRepository
{
    private readonly SQLiteAsyncConnection _sQLiteDb;

    public FriendRepository(SQLiteAsyncConnection sQLiteDb)
    {
        _sQLiteDb = sQLiteDb;
    }

    public async Task AddFriendAsync(Friend friend)
    {
        await _sQLiteDb.InsertAsync(friend);
    }

}
