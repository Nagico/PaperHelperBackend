using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Services.Serializers;

namespace PaperHelper.Services;

public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    private readonly UserSerializer _userSerializer;
    private readonly ProjectSerializer _projectSerializer;
    
    public UserService(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
        _userSerializer = new UserSerializer(_context);
        _projectSerializer = new ProjectSerializer(_context);
    }

    private User GetUser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            throw new AppError("A0514");
        }
        return user;
    }
    
    public JObject GetUserPartDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserInfo(user);
    }
    
    public JObject GetUserFullDetail(int id)
    {
        var user = GetUser(id);
        
        return _userSerializer.UserDetail(user);
    }
}