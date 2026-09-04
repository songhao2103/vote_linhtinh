namespace VoteLinhTinh.Datas.Entities;

public class Match : BaseEntity
{
    public int FirstSongId { get; set; }
    public int SecondSongId { get; set; }
    public int? WinnerSongId { get; set; }
    public int RoundId { get; set; }

    public Song FirstSong { get; } = new Song();
    public Song SecondSong { get; } = new Song();
    public Song? WinnerSong { get; }
    public Round Round { get; set; } = new Round();
}
