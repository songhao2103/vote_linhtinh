using Dapper;
using VoteLinhTinh.DTOs;
using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public class MatchRepository : BaseRepository, IMatchRepository
{
    public MatchRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<List<MatchDTO>> GetNextMatchesAsync(int? currentMatchId)
    {

        using var connection = CreateConnection();

        const string roundActiveSql = """
            SELECT
                id
            FROM rounds
            WHERE is_active = TRUE
            ORDER BY total_matches ASC
            """;
        var currentRoundId = await connection.QueryFirstAsync<int?>(roundActiveSql);

        if(!currentRoundId.HasValue)
        {
            throw new Exception("Round chưa được tạo");
        }

        const string matchsSql = """
            SELECT 
                m.id as Id,
                m.round_id as RoundId,
                m.song1_id as Song1Id,
                s1.name as Song1Name,
                m.song2_id as Song2Id,
                s2.name as Song2Name,
                m.match_order as MatchOrder,
                m.is_active as IsActive,
                s1.video_url as Song1VideoUrl,
                s2.video_url as Song2VideoUrl,
                s1.resource_url as Song1ResourceUrl,
                s2.resource_url as Song2ResourceUrl
            FROM matches m
            JOIN songs s1 ON m.song1_id = s1.id
            JOIN songs s2 ON m.song2_id = s2.id
            WHERE m.round_id = @RoundId
              AND m.winner_song_id IS NULL
              AND (
                    @CurrentMatchId IS NULL
                    OR m.match_order > (
                        SELECT match_order FROM matches WHERE id = @CurrentMatchId
                    )
                  )
            ORDER BY m.match_order ASC
            LIMIT 2
            """;

        var matches = await connection.QueryAsync<MatchDTO>(matchsSql, new { RoundId = currentRoundId.Value });
        return matches.ToList();
    }

    public async Task CreateMatchesByRound(int roundId)
    {
        using var connection = CreateConnection();
        const string roundSql = """
            SELECT
                id,
                total_matches
            FROM rounds
            WHERE total_matches > (SELECT total_matches FROM rounds WHERE id = @RoundId)
            LIMIT 1
            """;

        var round = await connection.QueryFirstOrDefaultAsync(roundSql, new { RoundId = roundId });

        if (round == null)
        {
            throw new Exception($"Round with id {roundId} not found.");
        }

        const string songsSql = """
            SELECT
                s.id,
                s.name,
                s.video_url,
                s.resource_url,
                s.is_active
            FROM matches m
            JOIN songs s ON s.id = m.winner_song_id
            WHERE (m.round_id = @RoundId)
              AND (m.winner_song_id IS NOT NULL)
            """;

        var songs = (await connection.QueryAsync<Song>(songsSql, new { RoundId = roundId })).ToList();
        var songIds = songs.Select(s => s.Id).ToList();
        var random = new Random();
        var matches = new List<Match>();
        int order = 1;

        while (songIds.Count >= 2)
        {
            var i1 = random.Next(songIds.Count);
            var id1 = songIds[i1];
            songIds.RemoveAt(i1);

            var i2 = random.Next(songIds.Count);
            var id2 = songIds[i2];
            songIds.RemoveAt(i2);

            matches.Add(new Match
            {
                RoundId = round.id,
                FirstSongId = id1,
                SecondSongId = id2,
                Order = order++
            });
        }

        const string insertMatchSql = """
            INSERT INTO matches (round_id, song1_id, song2_id, match_order)
            VALUES (@RoundId, @FirstSongId, @SecondSongId, @Order)
            """;

        await connection.ExecuteAsync(insertMatchSql, matches);
    }

    public async Task<bool> CompletedMatchAsync(int matchId, int winnerSongId)
    {
        using var connection = CreateConnection();
        try
        {
            const string sql = """
            UPDATE matches
            SET winner_song_id = @WinnerSongId
            WHERE id = @MatchId
            """;
            await connection.ExecuteAsync(sql, new { MatchId = matchId, WinnerSongId = winnerSongId });
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine("Có lỗi khi hoàn thành Match: " + ex.Message);
            return false;
        }
    }

    public async Task<Match?> GetMatchByIdAsync(int matchId)
    {
        using var connection = CreateConnection();
        const string sql = """
            SELECT
                id,
                round_id,
                song1_id,
                song2_id,
                winner_song_id,
                match_order,
                is_active
            FROM matches
            WHERE id = @MatchId
            """;
        return await connection.QueryFirstOrDefaultAsync<Match>(sql, new { MatchId = matchId });
    }
}
