namespace VoteLinhTinh.DTOs;
public class ExternalSongDTO
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsVideo { get; set; }
    public string VideoSource { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string ResourceUrl { get; set; } = string.Empty;
    public int StartTime { get; set; }
    public int EndTime { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public int FinalWins { get; set; }
    public int FinalLosses { get; set; }
    public double WinLossRatio { get; set; }
    public int GameId { get; set; }
    public int Ranking { get; set; }
}


public class ExternalSongResponse
{
    public List<ExternalSongDTO> Data { get; set; } = [];
}