using System.Text.Json;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.Infraestructure.Services;
public class JSONSerializable<A> : ISerializable<A>
{
    public A Deserialize(string dataToDeserialize)
    {
        return JsonSerializer.Deserialize<A>(dataToDeserialize);
    }

    public string Serialize(A dataToSerialize)
    {
        return JsonSerializer.Serialize(dataToSerialize);
    }
}
