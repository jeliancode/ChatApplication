using System.Text.Json;
using LookMeChatApp.Domain.Interface;

namespace LookMeChatApp.Infraestructure.Services;
public class JSONSerializable<A> : ISerializable<A>
{
    public A Deserialize(string dataToDeserialize)
    {
        try
        {
            return JsonSerializer.Deserialize<A>(dataToDeserialize);
        }
        catch (Exception e)
        {
            var error = e.Message;
        }
        return default;
    }

    public string Serialize(A dataToSerialize)
    {
        try
        {
            return JsonSerializer.Serialize(dataToSerialize);
        } catch (Exception e)
        {
            var error = e.Message;
        }
        return default;
    }
}
