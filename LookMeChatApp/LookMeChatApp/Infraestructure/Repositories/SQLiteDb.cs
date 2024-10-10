using LookMeChatApp.Domain.Model;
using SQLite;
using LookMeChatApp.Model;

namespace LookMeChatApp.Infraestructure.Repositories;
public class SQLiteDb
{
    private readonly SQLiteAsyncConnection _sQLiteAsync;
    public UserRepository UserRepository { get; set; }
    public MessageRepository MessageRepository { get; set; }

    public SQLiteDb(string dbPath)
    {
        _sQLiteAsync = new SQLiteAsyncConnection(dbPath);
        _sQLiteAsync.CreateTableAsync<User>().Wait();
        _sQLiteAsync.CreateTableAsync<ChatMessage>().Wait();
        _sQLiteAsync.CreateTableAsync<Friend>().Wait();

        UserRepository = new UserRepository(_sQLiteAsync);
        MessageRepository = new MessageRepository(_sQLiteAsync);
    }
}
