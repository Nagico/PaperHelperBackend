using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aliyun.OSS;
using Microsoft.AspNetCore.Http;

namespace PaperHelper.Utils;

/// <summary>
/// 阿里云上传帮助类。
/// </summary>
public class AliYunOssUtil
{
    // 阿里云oss相关参数
    private readonly string _endpoint;
    private readonly string _accessKeyId;
    private readonly string _accessKeySecret;
    private readonly string _bucket;
    private readonly OssClient _writeClient;
    private readonly OssClient _readClient;
    
    public AliYunOssUtil(IConfiguration configuration)
    {
        var aliYunConfig = configuration.GetSection("AliYun");

        _accessKeyId = aliYunConfig["AccessKeyId"];
        _accessKeySecret = aliYunConfig["AccessKeySecret"];
        _endpoint = aliYunConfig["Endpoint"];
        _bucket = aliYunConfig["Bucket"];

        // 初始化client
        _writeClient = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
        _readClient = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
    }
    
    /// <summary>
    /// IFormFile单文件上传
    /// </summary>
    /// <param name="path"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public Uri UploadFile(string path, IFormFile file)
    {
        try
        {
            var filename =  DateTime.Now.ToString("yyyyMMddHHmmssffffff") + Path.GetExtension(file.FileName);
            var key = Path.Combine(path, filename).Replace('\\', '/');
            if (key.StartsWith("/"))
            {
                key = key[1..];
            }
            using (var stream = file.OpenReadStream())
            {
                _writeClient.PutObject(_bucket, key, stream);
            }
            var expiration = DateTime.Now.AddYears(20);
            var url = _readClient.GeneratePresignedUri(_bucket, key, expiration);
            return url;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public void DeleteFile(string path, string filename)
    {
        var key = Path.Combine(path, filename).Replace('\\', '/');
        if (key.StartsWith("/"))
        {
            key = key[1..];
        }
        try
        {
            _writeClient.DeleteObject(_bucket, key);
        }
        catch (Exception ex)
        {
            // ignored
        }

    }

}