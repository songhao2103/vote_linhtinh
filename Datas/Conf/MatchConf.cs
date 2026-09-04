using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoteLinhTinh.Datas.Entities;

namespace VoteLinhTinh.Datas.Conf
{
    public class MatchConf : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.HasOne(x => x.FirstSong)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.FirstSongId);

            builder.HasOne(x => x.SecondSong)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.SecondSongId);

            builder.HasOne(x => x.WinnerSong)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.WinnerSongId);

            builder.HasOne(x => x.Round)
                .WithMany(x => x.Matches)
                .HasForeignKey(x => x.RoundId);
        }
    }
}
