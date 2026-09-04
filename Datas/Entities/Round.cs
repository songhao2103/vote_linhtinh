namespace VoteLinhTinh.Datas.Entities;

public class Round : BaseEntity
{
    public int TotalMatch { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Match> Matches { get; set; } = new List<Match>();
}
