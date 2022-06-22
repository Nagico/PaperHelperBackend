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
    
    public AliYunOssUtil(string accessKeyId, 
        string accessKeySecret, string endpoint, string bucket)
    {
        _endpoint = endpoint;
        _accessKeyId = accessKeyId;
        _accessKeySecret = accessKeySecret;
        _bucket = bucket;

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
            var fileName =  DateTime.Now.ToString("yyyyMMddHHmmssffffff") + Path.GetExtension(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                _writeClient.PutObject(_bucket, path + '/' + fileName, stream);
            }
            var expiration = DateTime.Now.AddYears(20);
            var url = _readClient.GeneratePresignedUri(_bucket, fileName, expiration);
            return url;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
    /// <summary>
    /// IFormFile多文件上传
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    public List<Uri> UploadFile(List<IFormFile> files)
    { 
        var ossFilesNameList = new List<Uri>();
        try
        {
            var writeClient = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
            var readClient = new OssClient(_endpoint, _accessKeyId, _accessKeySecret);
           
            for (var i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var fileName =  DateTime.Now.ToString("yyyyMMddHHmmssffffff") + i + Path.GetExtension(file.FileName);
                using (var stream = file.OpenReadStream())
                {
                    writeClient.PutObject(_bucket, fileName, stream);
                }
                var expiration = DateTime.Now.AddYears(20);
                var url = readClient.GeneratePresignedUri(_bucket, fileName, expiration);
                ossFilesNameList.Add(url);
            }
            
        }
        catch (Exception ex)
        {
            throw;
        }

        return ossFilesNameList;
    }

}