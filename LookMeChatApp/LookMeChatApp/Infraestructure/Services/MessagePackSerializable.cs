using MessagePack;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Infraestructure.Services;
public class MessagePackSerializable : ISerializable<ChatMessage>
{
    public ChatMessage Deserialize(string dataToDeserialize)
    {
        byte[] data = Convert.FromBase64String(dataToDeserialize);
        return MessagePackSerializer.Deserialize<ChatMessage>(data);
    }

    public string Serialize(ChatMessage dataToSerialize)
    {
        return Convert.ToBase64String(MessagePackSerializer.Serialize(dataToSerialize));
    }
}
