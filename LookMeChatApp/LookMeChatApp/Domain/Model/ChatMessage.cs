using SQLite;

namespace LookMeChatApp.Domain.Model;
public class ChatMessage
{
    [PrimaryKey, Unique]
    public Guid Id { get; set; }
    [NotNull]
    public Guid SenderId { get; set; }
    [NotNull]
    public string Message { get; set; }
    [NotNull]
    public string Room { get; set; }
    [NotNull]
    public string Timestamp { get; set; }
}
