using VoteLinhTinh.DTOs;
using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public interface IMatchRepository
{
    public Task<List<MatchDTO>> GetNextMatchesAsync(int? currentMatchId);
    public Task CreateMatchesByRound(int roundId);
    public Task<bool> CompletedMatchAsync(int matchId, int winnerSongId);
    public Task<Match?> GetMatchByIdAsync(int matchId);
}
