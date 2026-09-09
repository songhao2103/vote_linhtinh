using VoteLinhTinh.DTOs;
using VoteLinhTinh.Repositories;

namespace VoteLinhTinh.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    private readonly IRoundRepository _roundRepository;

    public MatchService(IMatchRepository matchRepository, IRoundRepository roundRepository)
    {
        _matchRepository = matchRepository;
        _roundRepository = roundRepository;
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
        var match = await _matchRepository.GetMatchByIdAsync(matchId);
        if (match == null)
        {
            throw new Exception($"Match with id {matchId} not found.");
        }

        var rounds = await _roundRepository.GetAllRoundsAsync();
        rounds.Sort((r1, r2) => r1.TotalMatches.CompareTo(r2.TotalMatches));
        var currentRound = rounds.FirstOrDefault(r => r.Id == match.RoundId);

        if(currentRound == null)
        {
            throw new Exception($"Round with id {match.RoundId} not found.");
        }

        var nextRound = rounds.FirstOrDefault(r => r.TotalMatches < currentRound?.TotalMatches);

        var result = await _matchRepository.CompletedMatchAsync(matchId, winnerSongId);

        if(!result)
        {
            throw new Exception($"Failed to complete match with id {matchId}.");
        }

        if(match.Order == currentRound.TotalMatches && nextRound != null)
        {
            await _roundRepository.DeactivateRoundAsync(currentRound.Id);
            await _matchRepository.CreateMatchesByRound(nextRound.Id); 
        }
    }
}
