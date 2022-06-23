using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Services.Serializers;
using PaperHelper.Utils;

namespace PaperHelper.Services;

/// <summary>
/// 附件管理服务
/// </summary>
public class AttachmentService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly AttachmentSerializer _attachmentSerializer;
    private readonly AliYunOssUtil _ali;
    
    public AttachmentService(IConfiguration configuration, PaperHelperContext context)
    {
        _configuration = configuration;
        _context = context;
        _attachmentSerializer = new AttachmentSerializer(_context);
        
        _ali = new AliYunOssUtil(_configuration);
    }
    
    /// <summary>
    /// 上传附件
    /// </summary>
    /// <param name="projectId">项目ID</param>
    /// <param name="fileName">文件名</param>
    /// <param name="extName">扩展名</param>
    /// <param name="formFile">文件</param>
    /// <returns>附件对象</returns>
    public Attachment CreateAttachment(int projectId, string fileName, string extName, IFormFile formFile)
    {
        var md5 = EncryptionUtil.Md5File(formFile);
        // 上传附件
        var url = _ali.UploadFile($"project/{projectId}", formFile);

        // 新增附件信息
        var attachment = new Attachment
        {
            Name = fileName,
            Ext = extName,
            Url = url,
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now
        };
        
        // 保存
        _context.Attachments.Add(attachment);
        _context.SaveChanges();
        
        // 返回附件信息
        return attachment;
    }
    
}