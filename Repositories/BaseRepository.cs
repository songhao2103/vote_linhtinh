using Npgsql;

namespace VoteLinhTinh.Repositories;

public abstract class BaseRepository
{
    private readonly IConfiguration _configuration;

    public BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected NpgsqlConnection CreateConnection()
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        return new NpgsqlConnection(connectionString);
    }
}
