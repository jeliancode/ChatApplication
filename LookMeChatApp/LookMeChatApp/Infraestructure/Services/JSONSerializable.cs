using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
