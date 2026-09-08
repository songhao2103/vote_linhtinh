using Dapper;
using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public class RoundRepository : BaseRepository, IRoundRepository
{
    public RoundRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Round> GetRoundActiveAsync()
    {
        using var connection = CreateConnection();

        const string sql = """
            SELECT
                id as Id,
                name as Name,
                total_matches as TotalMatches,
                is_active as IsActive,
                created_at as CreatedAt
            FROM rounds
            WHERE is_active = true
            """;
        var round = await connection.QueryFirstOrDefaultAsync<Round>(sql);

        if(round == null)
        {
            throw new Exception("No active round found.");
        }

        return round;
    }

    public async Task CreateRoundAsync(int totalMatches)
    {
        using var connection = CreateConnection();

        const string totalSongSql = """
            SELECT COUNT(*) FROM songs
            WHERE is_active = TRUE
            """;
        var count = await connection.ExecuteScalarAsync<int>(totalSongSql);

        if(count < totalMatches * 2)
        {
            throw new Exception("Not enough active songs to create the round.");
        }

        const string createRoundSql = """
            INSERT INTO rounds (name, total_matches, is_active, created_at)
            VALUES (@Name, @TotalMatches, TRUE, NOW())
            """;

        var rounds = new List<Round>();
        var matchCount = totalMatches;

        while(matchCount < 2)
        {
            var round = new Round
            {
                Name = $"Vòng 1:{matchCount}",
                TotalMatches = matchCount,
                IsActive = matchCount == totalMatches
            };

            rounds.Add(round);
            matchCount = matchCount / 2;
        }

        await connection.ExecuteAsync(createRoundSql, rounds);
    }

    public async Task DeactivateRoundAsync(int roundId)
    {
        using var connection = CreateConnection();
        const string sql = """
            UPDATE rounds
            SET is_active = false
            WHERE id = @RoundId
            """;
        await connection.ExecuteAsync(sql, new { RoundId = roundId });
    }
}
