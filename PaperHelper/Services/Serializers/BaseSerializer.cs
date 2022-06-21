using PaperHelper.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// 序列化器基类
/// </summary>
public class BaseSerializer
{
    protected readonly PaperHelperContext _context;

    protected BaseSerializer(PaperHelperContext context)
    {
        _context = context;
    }
}