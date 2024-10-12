using LookMeChatApp.Domain.Model;
using SQLite;

namespace LookMeChatApp.Infraestructure.Repositories;
public class RoomRepository
{
    private readonly SQLiteAsyncConnection _sQLiteDb;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public RoomRepository(SQLiteAsyncConnection sQLiteDb)
    {
        _sQLiteDb = sQLiteDb;
    }

    public async Task AddRoomAsync(Room room)
    {
        await _semaphore.WaitAsync();
        try
        {
            await _sQLiteDb.InsertAsync(room);
        }
        finally
        {
            _semaphore.Release();
        };
    }

    public async Task<Room?> FindByUsernameAsync(string roomName)
    {
        return await _sQLiteDb.Table<Room>()
            .FirstOrDefaultAsync(x => x.RoomName == roomName);
    }

    public async Task<List<Room>> GetAllRoomsAsync()
    {
        return await _sQLiteDb.Table<Room>().ToListAsync();
    }
}
