using Dapper;
using System.Text.RegularExpressions;

namespace VoteLinhTinh.Repositories
{
    public class MatchRepository : BaseRepository, IMatchRepository
    {
        public MatchRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<List<Match>> GetAllMatchsAsync(int matchId)
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
                //TODO: Tạo round mới nếu chưa có round nào
                throw new Exception("Round chưa được tạo");
            }

            var sql = """

                """
        }
    }
}
