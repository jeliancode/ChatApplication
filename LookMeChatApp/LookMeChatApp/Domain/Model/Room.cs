using SQLite;

namespace LookMeChatApp.Domain.Model;
public class Room
{
    [PrimaryKey, Unique]
    public Guid Id { get; set; }
    [NotNull, Unique]
    public string RoomName { get; set; }
    public Guid UserId { get; set; }
}
