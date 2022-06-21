using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// 用户序列化器
/// </summary>
public class UserSerializer : BaseSerializer
{
    public UserSerializer(PaperHelperContext context) : base(context) {}
    
    /// <summary>
    /// 获取用户详细信息
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>JSON对象</returns>
    public JObject UserDetail(User user)
    {
        var res = JObject.FromObject(user);
        var projectNum = _context.UserProjects.Count(x => x.UserId == user.Id);

        res.Remove("Password");
        res.Add("project_num", projectNum);
        return res;
    }
    
    /// <summary>
    /// 获取用户简要信息
    /// </summary>
    /// <param name="user">用户</param>
    /// <returns>JSON对象</returns>
    public JObject UserInfo(User user)
    {
        var res = JObject.FromObject(user);
        
        res.Remove("password");
        res.Remove("phone");
        res.Remove("last_login");
        res.Remove("create_time");
        return res;
    }
}