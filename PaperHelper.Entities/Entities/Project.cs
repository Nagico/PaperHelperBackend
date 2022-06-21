using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public List<UserProject>? Members { get; set; }
    [JsonIgnore]
    public List<Attachment>? Attachments { get; set; }
    [JsonIgnore]
    public List<Note>? Notes { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    
}