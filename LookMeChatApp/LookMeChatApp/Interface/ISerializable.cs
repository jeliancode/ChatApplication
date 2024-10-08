using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookMeChatApp.Interface;
public interface ISerializable<A>
{
    string Serialize(A dataToSerialize);
    A Deserialize(string dataToDeserialize);
}
