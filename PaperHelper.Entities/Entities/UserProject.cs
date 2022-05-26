namespace PaperHelper.Entities.Entities;

public class UserProject
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    
    public int ProjectId { get; set; }
    public virtual Project? Project { get; set; }
    
    public bool IsOwner { get; set; }
    
    public DateTime? AccessTime { get; set; }
    public DateTime? EditTime { get; set; }
    public DateTime? CreateTime { get; set; }
}