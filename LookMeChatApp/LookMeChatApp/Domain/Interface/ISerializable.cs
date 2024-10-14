namespace LookMeChatApp.Domain.Interface;
public interface ISerializable<A>
{
    string Serialize(A dataToSerialize);
    A Deserialize(string dataToDeserialize);
}
