namespace VoteLinhTinh.Datas.Entities;

public class Song : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<Match> Matches { get; set; } = new List<Match>();
}
