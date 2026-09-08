namespace VoteLinhTinh.Models;

public class Round : BaseModel
{
    public int TotalMatches { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
