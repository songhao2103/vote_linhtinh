namespace VoteLinhTinh.Models;

public class Match : BaseModel
{
    public int FirstSongId { get; set; }
    public int SecondSongId { get; set; }
    public int? WinnerSongId { get; set; }
    public int RoundId { get; set; }
    public int Order { get; set; }
}
