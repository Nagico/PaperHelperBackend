using System.Security.Cryptography;
using System.Text;

namespace PaperHelper.Utils;

public static class EncryptionUtil
{
    private static string Md5(string source)
    {
        using var md5 = MD5.Create();
        var strResult = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(source)));
        return strResult.Replace("-", "");
    }
    
    public static string Encrypt(string text, string salt)
    {
        return Md5(Md5(text) + salt);
    }
}