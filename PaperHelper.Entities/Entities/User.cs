using Newtonsoft.Json;

namespace PaperHelper.Entities.Entities;

public class User
{
    public int Id { get; set; }
    public string? Username { get; set; }
    [JsonIgnore]
    public string? Password { get; set; }
    public string? Phone { get; set; }
    
    public Uri? Avatar { get; set; }
    [JsonIgnore]
    public List<UserProject>? UserProjects { get; set; }

    public DateTime LastLogin { get; set; }
    public DateTime CreateTime { get; set; }
}