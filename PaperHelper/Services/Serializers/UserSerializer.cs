using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

public class UserSerializer : BaseSerializer
{
    public UserSerializer(PaperHelperContext context) : base(context) {}

    public JObject UserInfo(User user)
    {
        var res = JObject.FromObject(user);
        var projectNum = _context.UserProjects.Count(x => x.UserId == user.Id);

        res.Remove("Password");
        res.Add("project_num", projectNum);
        return res;
    }
    
    public JObject UserDetail(User user)
    {
        var res = JObject.FromObject(user);
        
        res.Remove("password");
        res.Remove("phone");
        res.Remove("email");
        res.Remove("last_login");
        res.Remove("create_time");
        return res;
    }
}