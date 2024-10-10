using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LookMeChatApp.Model;

namespace LookMeChatApp.Domain.Interface;
public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> FindByIdAsync(Guid id);
    Task<User?> FindByUsernameAsync(string username);
}
