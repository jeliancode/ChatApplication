using SQLite;

namespace LookMeChatApp.Domain.Model;
public class Room
{
    [PrimaryKey, Unique]
    public Guid Id { get; set; }
    [NotNull]
    public string RoomName { get; set; }
    [NotNull]
    public string topicPath {  get; set; }
}
