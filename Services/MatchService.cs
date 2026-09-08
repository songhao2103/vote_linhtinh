using VoteLinhTinh.DTOs;
using VoteLinhTinh.Repositories;

namespace VoteLinhTinh.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;

    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<List<MatchDTO>> GetNextMatchesAsync(int? currentMatchId)
    {
        return await _matchRepository.GetNextMatchesAsync(currentMatchId);
    }

    public async Task CreateMatchesByRound(int roundId)
    {
        await _matchRepository.CreateMatchesByRound(roundId);
    }

    public async Task CompletedMatchAsync(int matchId, int winnerSongId)
    {
        var match = await
        await _matchRepository.CompletedMatchAsync(matchId, winnerSongId);
    }
}
