using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services;

public class UserService
{
    private readonly IConfiguration _configuration;
    private readonly PaperHelperContext _context;
    
    public UserService(IConfiguration configuration, PaperHelperContext paperHelperContext)
    {
        _configuration = configuration;
        _context = paperHelperContext;
    }

    public User? GetUser(int id)
    {
        return _context.Users.Find(id);
    }

    public JObject GetUserDetail(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return new JObject();
        }
        var projectNum = _context.UserProjects.Count(x => x.UserId == id);
        var res = JObject.FromObject(user);
        res.Remove("password");
        res.Add("project_num", projectNum);
        return res;
    }
}