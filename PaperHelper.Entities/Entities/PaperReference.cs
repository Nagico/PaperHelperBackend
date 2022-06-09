namespace PaperHelper.Entities.Entities;

public class PaperReference
{
    public int Id { get; set; }
    
    public int PaperId { get; set; }
    public virtual Paper? Paper { get; set; }
    
    public int RefPaperId { get; set; }
    public virtual Paper? RefPaper { get; set; }
}