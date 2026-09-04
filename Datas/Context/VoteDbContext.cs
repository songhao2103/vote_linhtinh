using Microsoft.EntityFrameworkCore;

namespace VoteLinhTinh.Datas.Context;

public class VoteDbContext : DbContext
{
    public VoteDbContext(DbContextOptions<VoteDbContext> options) : base(options)
    {
    }


}
