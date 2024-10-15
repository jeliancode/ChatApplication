using Konscious.Security.Cryptography;
using System.Text;

namespace LookMeChatApp.Infraestructure.Services;
public class HashService
{
    public byte[] HashPassword(string password, byte[] salt)
    {

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        using (var hasher = new Argon2id(passwordBytes))
        {
            hasher.Salt = salt;
            hasher.MemorySize = 65536;
            hasher.Iterations = 4;
            hasher.DegreeOfParallelism = 8;

            return hasher.GetBytes(32);
        }
    }
}
