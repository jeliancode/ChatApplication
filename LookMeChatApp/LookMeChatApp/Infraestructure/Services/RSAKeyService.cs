using System.Security.Cryptography;
using System.Text;

namespace LookMeChatApp.Infraestructure.Services;
public class RSAKeyService
{
    private RSA rsa;

    public RSAKeyService()
    {
        rsa = RSA.Create(2048);
    }

    public byte[] EncryptWithPublicKey(byte[] aesKey, string publicKey)
    {
        var encryptRsa = RSA.Create();
        encryptRsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
        return encryptRsa.Encrypt(aesKey, RSAEncryptionPadding.OaepSHA256);
    }

    public byte[] DecryptWithPrivateKey(string privateKey, byte[] data)
    {
        var decryptRsa = RSA.Create();
        decryptRsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
        try
        { 
            return decryptRsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256); 
        }catch (Exception ex)
        {
            var error = ex.Message;
        }
        return null ;
    }

    public string GetPublicKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPublicKey());
    }

    public string GetPrivateKey()
    {
        return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
    }
}
