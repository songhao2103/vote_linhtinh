namespace VoteLinhTinh.DTOs;

public class MatchDTO
{
    public int Id { get; set; }
    public int Song1Id { get; set; }
    public int Song2Id { get; set; }
    public string Song1Name { get; set; } = string.Empty;
    public string Song2Name { get; set; } = string.Empty;
    public string Song1VideoUrl { get; set; } = string.Empty;
    public string Song2VideoUrl { get; set; } = string.Empty;
    public string Song1ResourceUrl { get; set; } = string.Empty;
    public string Song2ResourceUrl { get; set; } = string.Empty;
    public int MatchOrder { get; set; }
    public int RoundId { get; set; }
}
