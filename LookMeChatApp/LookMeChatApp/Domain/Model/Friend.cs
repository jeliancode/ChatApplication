using SQLite;

namespace LookMeChatApp.Domain.Model;
public class Friend
{
    [PrimaryKey, Unique]
    public Guid Id { get; set; }
    [NotNull]
    public string Username { get; set; }
}
