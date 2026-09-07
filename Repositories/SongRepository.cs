using Dapper;
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
                    id,
                    name,
                    videoUrl,
                    resourceUrl,
                    isActive,
                    createdAt
                FROM songs
                WHERE (@searchKey IS NULL OR name ILIKE '%' || @searchKey || '%')
                  AND (@isActive IS NULL OR isActive = @isActive)
                ORDER BY createdAt DESC
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
                    id,
                    name,
                    videoUrl,
                    resourceUrl,
                    isActive,
                    createdAt
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
        try
        {
            if (songs == null || songs.Count == 0)
                return 0;

            using var connection = CreateConnection();

            const string sql = """
                INSERT INTO songs
                (
                    name,
                    videoUrl,
                    resourceUrl,
                    isActive,
                    createdAt
                )
                VALUES
                (
                    @Name,
                    @VideoUrl,
                    @ResourceUrl,
                    @IsActive,
                    @CreatedAt
                )
                ON CONFLICT (name, videoUrl)
                DO UPDATE SET
                    resourceUrl = EXCLUDED.resourceUrl,
                    isActive = EXCLUDED.isActive
                RETURNING id;
                """;

            var result = await connection.QueryAsync<int>(sql, songs);

            return result.Count();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error while upserting songs. Song count: {SongCount}",
                songs?.Count ?? 0);

            throw;
        }
    }
}