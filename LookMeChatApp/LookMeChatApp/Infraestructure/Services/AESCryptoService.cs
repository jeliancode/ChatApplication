using System.Security.Cryptography;
using System.Text;

namespace LookMeChatApp.Infraestructure.Services;
public class AESCryptoService
{
    private Aes aes;

    public AESCryptoService()
    {
        aes = Aes.Create();
        aes.KeySize = 256;
        aes.GenerateKey();
        aes.GenerateIV();
    }

    public byte[] EncryptMessage(string message)
    {
        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        byte[] plainText = Encoding.UTF8.GetBytes(message);
        return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
    }

    public string DecryptMessage(byte[] encryptedMessage)
    {
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedMessage, 0, encryptedMessage.Length);
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    public byte[] GetKey()
    {
        return aes.Key;
    }

    public byte[] GetIV()
    {
        return aes.IV;
    }

}
