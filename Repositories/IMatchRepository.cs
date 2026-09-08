using VoteLinhTinh.DTOs;

namespace VoteLinhTinh.Repositories;

public interface IMatchRepository
{
    public Task<List<MatchDTO>> GetNextMatchesAsync(int? currentMatchId);
    public Task CreateMatchesByRound(int roundId);
    public Task CompletedMatchAsync(int matchId, int winnerSongId);
}
