namespace LookMeChatApp.Model;
public class User
{
    public Guid IdUser { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Description { get; set; }
    public bool IsOnline { get; set; }
}
