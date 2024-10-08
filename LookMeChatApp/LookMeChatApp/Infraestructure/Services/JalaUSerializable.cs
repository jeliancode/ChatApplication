using System.Text;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Infraestructure.Services;
public class JalaUSerializable : ISerializable<ChatMessage>
{
    public ChatMessage Deserialize(string dataToDeserialize)
    {
        byte[] messageBytes = Convert.FromBase64String(dataToDeserialize);
        Guid messageId = new Guid(messageBytes.Take(10).ToArray());
        Guid senderId = new Guid(messageBytes.Skip(10).Take(32).ToArray());
        string room = Encoding.UTF8.GetString(messageBytes.Skip(42).Take(32).ToArray());
        string messageContent = Encoding.UTF8.GetString(messageBytes.Skip(74).Take(messageBytes.Length - 74 - 20).ToArray());
        var timestamp = Encoding.UTF8.GetString(messageBytes.Skip(74 + messageContent.Length).ToArray());

        return new ChatMessage
        {
            Id = messageId,
            SenderId = senderId,
            Room = room,
            Message = messageContent,
            Timestamp = timestamp
        };
    }

    public string Serialize(ChatMessage dataToSerialize)
    {
        byte[] messageIdBytes = dataToSerialize.Id.ToByteArray().Take(10).ToArray();
        byte[] senderIdBytes = dataToSerialize.SenderId.ToByteArray().Take(32).ToArray();
        byte[] roomBytes = Encoding.UTF8.GetBytes(dataToSerialize.Room).Take(32).ToArray();
        byte[] messageContentBytes = Encoding.UTF8.GetBytes(dataToSerialize.Message);
        byte[] timestampBytes = Encoding.UTF8.GetBytes(dataToSerialize.Timestamp);

        byte[] bytesCombined = messageIdBytes
            .Concat(senderIdBytes)
            .Concat(roomBytes)
            .Concat(messageContentBytes)
            .Concat(timestampBytes)
            .ToArray();

        return Convert.ToBase64String(bytesCombined);
    }
}
