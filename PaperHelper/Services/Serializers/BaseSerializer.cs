using PaperHelper.Entities;

namespace PaperHelper.Services.Serializers;

public class BaseSerializer
{
    protected readonly PaperHelperContext _context;
    
    public BaseSerializer(PaperHelperContext context)
    {
        _context = context;
    }
}