using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Domain.Interface;
public interface IPathBuilder
{
    string ConnectionPath(string version, Room room);
    string UserPath(string username);
}
