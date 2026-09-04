using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VoteLinhTinh.Datas.Entities;

namespace VoteLinhTinh.Datas.Conf;

public class SongConf : IEntityTypeConfiguration<Song>
{
    public void Configure(EntityTypeBuilder<Song> builder)
    {
    }
}
