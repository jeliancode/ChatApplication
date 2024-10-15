using System.Threading;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;
using SQLite;

namespace LookMeChatApp.Infraestructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly SQLiteAsyncConnection _sQLiteDb;
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public MessageRepository(SQLiteAsyncConnection sQLiteDb)
    {
        _sQLiteDb = sQLiteDb;
    }

    public async Task AddMessageAsync(ChatMessage message)
    {
        await _semaphore.WaitAsync();
        try
        {
            try
            {
                await _sQLiteDb.InsertAsync(message);
            }
            catch (Exception ex) 
            { 
            }
                
        }
        finally
        {
            _semaphore.Release();
        };
    }

    public async Task<List<ChatMessage>> GetMessagesByRoom(string roomName)
    {
        return await _sQLiteDb.Table<ChatMessage>()
            .Where(m => m.Room == roomName)
            .OrderBy(m => m.Timestamp).ToListAsync();
    }

    public async Task<List<ChatMessage>> GetAllMessagesAsync(string room)
    {
        return await _sQLiteDb.Table<ChatMessage>()
            .Where(m => m.Room == room).ToListAsync();
    }
}
