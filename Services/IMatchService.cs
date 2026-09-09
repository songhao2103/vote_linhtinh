namespace VoteLinhTinh.Services;

using VoteLinhTinh.DTOs;

public interface IMatchService
{
    public Task<List<MatchDTO>> GetNextMatchesAsync(int? currentMatchId);
    public Task CreateMatchesByRound(int roundId);
    public Task CompletedMatchAsync(int matchId, int winnerSongId);
}
