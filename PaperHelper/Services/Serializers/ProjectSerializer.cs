using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

/// <summary>
/// 项目序列化器
/// </summary>
public class ProjectSerializer : BaseSerializer
{
    public ProjectSerializer(PaperHelperContext context) : base(context) {}
    
    /// <summary>
    /// 获取项目简要信息
    /// </summary>
    /// <param name="project">项目</param>
    /// <returns>JSON列表</returns>
    public JObject ProjectInfo(Project project)
    {
        var jObject = new JObject
        {
            {"id", project.Id},
            {"name", project.Name},
            {"description", project.Description}, 
            {"update_time", project.UpdateTime}
        };
        var members = new JArray();
        foreach (var member in project.Members)
        {
            var user = _context.Users.Find(member.UserId);
            if (user != null)
            {
                members.Add(new JObject {
                    {"id", user.Id},
                    {"is_owner", member.IsOwner},
                    {"avatar", user.Avatar},
                    {"edit_time", member.EditTime}
                });
            }
        }
        jObject.Add("members", members);
        return jObject;
    }
    
    /// <summary>
    /// 获取项目详细信息
    /// </summary>
    /// <param name="project">项目</param>
    /// <returns>JSON对象</returns>
    public JObject ProjectDetail(Project project)
    {
        var jObject = JObject.FromObject(project);
        var members = new JArray();
        
        // 项目成员
        foreach (var member in project.Members)
        {
            var user = _context.Users.Find(member.UserId);
            if (user != null)
            {
                members.Add(new JObject {
                    {"id", user.Id},
                    {"username", user.Username},
                    {"is_owner", member.IsOwner},
                    {"phone", user.Phone},
                    {"avatar", user.Avatar},
                    {"last_login", user.LastLogin},
                    {"access_time", member.AccessTime},
                    {"edit_time", member.EditTime}
                });
            }
        }
        jObject.Add("members", members);
        
        // 项目论文
        var papers = new JArray();
        var paperSerializer = new PaperSerializer(_context);
        foreach (var paper in project.Papers)
        {
            papers.Add(paperSerializer.PaperInfo(paper));
        }
        jObject.Add("paper", papers);
        
        return jObject;
    }
}