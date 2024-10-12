using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Interface;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Infraestructure.Services;
public class PathBuildService : IPathBuilder
{
    public string ConnectionPath(string version, Room room)
    {
        return $"/{version}/room/+/{room.RoomName}";
    }

    public string UserPath(string username)
    {
        return $"/v1/room/{username}/messages";
    }
}
