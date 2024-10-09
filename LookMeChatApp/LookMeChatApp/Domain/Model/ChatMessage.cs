namespace LookMeChatApp.Domain.Model;
public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid SenderId { get; set; }
    public string Message { get; set; }
    public string Room { get; set; }
    public string Timestamp { get; set; }
}
