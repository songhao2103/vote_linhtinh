namespace VoteLinhTinh.Models;

public class DatabaseInitializer
{
    private readonly IConfiguration _configuration;

    public DatabaseInitializer(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        var connectionString =
            _configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' không được cấu hình.");
        }

        await using var connection =
            new Npgsql.NpgsqlConnection(connectionString);

        await connection.OpenAsync();

        var sqlFiles = Directory.GetFiles("Database", "*.sql")
            .OrderBy(f => f)
            .ToList();

        foreach (var sqlFile in sqlFiles)
        {
            var sql = await File.ReadAllTextAsync(sqlFile);

            await using var command =
                new Npgsql.NpgsqlCommand(sql, connection);

            await command.ExecuteNonQueryAsync();
        }
    }
}