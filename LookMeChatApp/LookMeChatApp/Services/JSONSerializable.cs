using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LookMeChatApp.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LookMeChatApp.Services;
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
