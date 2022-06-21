using System.Security.Cryptography;
using System.Text;

namespace PaperHelper.Utils;

/// <summary>
/// 加密
/// </summary>
public static class EncryptionUtil
{
    /// <summary>
    /// MD5摘要算法
    /// </summary>
    /// <param name="source">原文</param>
    /// <returns>加密</returns>
    private static string Md5(string source)
    {
        using var md5 = MD5.Create();
        var strResult = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(source)));
        return strResult.Replace("-", "");
    }
    
    /// <summary>
    /// 带盐加密
    /// </summary>
    /// <param name="text">待加密文本</param>
    /// <param name="salt">盐值</param>
    /// <returns>加密后文本</returns>
    public static string Encrypt(string text, string salt)
    {
        return Md5(Md5(text) + salt);
    }
}