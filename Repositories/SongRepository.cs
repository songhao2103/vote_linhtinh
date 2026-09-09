using Dapper;
using System.Text;
using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public class SongRepository : BaseRepository, ISongRepository
{
    private readonly ILogger<SongRepository> _logger;

    public SongRepository(
        IConfiguration configuration,
        ILogger<SongRepository> logger) : base(configuration)
    {
        _logger = logger;
    }

    public async Task<List<Song>> GetAllSongsAsync(
        int pageIndex,
        int pageSize,
        string? searchKey,
        bool? isActive)
    {
        try
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = (pageSize < 1 || pageSize > 1000) ? 10 : pageSize;

            var offset = (pageIndex - 1) * pageSize;

            using var connection = CreateConnection();

            const string sql = """
                SELECT
                    id as Id,
                    name as Name,
                    video_url as VideoUrl,
                    resource_url as ResourceUrl,
                    is_active as IsActive,
                    created_at as CreatedDate
                FROM songs
                WHERE (@searchKey IS NULL OR name ILIKE '%' || @searchKey || '%')
                  AND (@isActive IS NULL OR is_active = @isActive)
                ORDER BY created_at DESC
                LIMIT @pageSize
                OFFSET @offset
                """;

            var songs = await connection.QueryAsync<Song>(
                sql,
                new
                {
                    searchKey,
                    isActive,
                    pageSize,
                    offset
                });

            return songs.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while getting songs. PageIndex: {PageIndex}, PageSize: {PageSize}, SearchKey: {SearchKey}, IsActive: {IsActive}",
                pageIndex,
                pageSize,
                searchKey,
                isActive);

            throw;
        }
    }

    public async Task<Song?> GetSongByIdAsync(int id)
    {
        try
        {
            using var connection = CreateConnection();

            const string sql = """
                SELECT
                    id as Id,
                    name as Name,
                    video_url as VideoUrl,
                    resource_url as ResourceUrl,
                    is_active as IsActive,
                    created_at as CreatedDate
                FROM songs
                WHERE id = @id
                """;

            return await connection.QuerySingleOrDefaultAsync<Song>(
                sql,
                new { id });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while getting song by id: {Id}",
                id);

            throw;
        }
    }

    public async Task<int> UpsertSongsAsync(List<Song> songs)
    {
        if (songs == null || songs.Count == 0)
            return 0;

        try
        {
            using var connection = CreateConnection();

            var sqlBuilder = new StringBuilder("""
            INSERT INTO songs
            (
                name,
                video_url,
                resource_url,
                is_active,
                created_at
            )
            VALUES
            """);

            var parameters = new DynamicParameters();

            var values = new List<string>();

            for (int i = 0; i < songs.Count; i++)
            {
                var song = songs[i];

                values.Add(
                    $"(@Name{i}, @VideoUrl{i}, @ResourceUrl{i}, @IsActive{i}, @CreatedDate{i})"
                );

                parameters.Add($"Name{i}", song.Name);
                parameters.Add($"VideoUrl{i}", song.VideoUrl);
                parameters.Add($"ResourceUrl{i}", song.ResourceUrl);
                parameters.Add($"IsActive{i}", song.IsActive);
                parameters.Add($"CreatedDate{i}", song.CreatedDate);
            }

            sqlBuilder.AppendLine(string.Join(",\n", values));

            sqlBuilder.AppendLine("""
            ON CONFLICT (name, video_url)
            DO UPDATE SET
                resource_url = EXCLUDED.resource_url,
                is_active = EXCLUDED.is_active;
            """);

            var sql = sqlBuilder.ToString();

            var affectedRows = await connection.ExecuteAsync(
                sql,
                parameters
            );

            return affectedRows;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while upserting songs. Song count: {SongCount}",
                songs.Count
            );

            throw;
        }
    }
}