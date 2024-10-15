using SQLite;

namespace LookMeChatApp.Model;
public class User
{
    [PrimaryKey, Unique]
    public Guid IdUser { get; set; }
    [NotNull, Unique]
    public string Username { get; set; }
    [NotNull]
    public string Password { get; set; }
    public string Description { get; set; }
    public string Salt { get; set; }
}
