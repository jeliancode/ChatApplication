using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Domain.Model;

namespace LookMeChatApp.Domain.Interface;
public interface IMessageRepository
{
    Task AddMessageAsync(ChatMessage message);
    Task<List<ChatMessage>> GetAllMessagesAsync(string room);
}
