namespace VoteLinhTinh.Models;

public class Song : BaseModel
{
    public string Name { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
