using Newtonsoft.Json.Linq;
using PaperHelper.Entities;
using PaperHelper.Entities.Entities;

namespace PaperHelper.Services.Serializers;

public class ProjectSerializer : BaseSerializer
{
    public ProjectSerializer(PaperHelperContext context) : base(context) {}

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

    public JObject ProjectDetail(Project project)
    {
        var jObject = JObject.FromObject(project);
        var members = new JArray();
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
        return jObject;
    }
}