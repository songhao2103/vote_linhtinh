namespace VoteLinhTinh.Models;

public class Round : BaseModel
{
    public int TotalMatch { get; set; }
    public string Name { get; set; } = string.Empty;
}
