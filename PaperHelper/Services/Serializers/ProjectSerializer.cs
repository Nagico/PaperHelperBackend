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
        return jObject;
    }

    // public JObject ProjectDetail(Project project)
    // {
    //     //var jObject = new 
    // }
}