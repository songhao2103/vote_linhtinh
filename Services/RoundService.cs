using VoteLinhTinh.Repositories;

namespace VoteLinhTinh.Services;

public class RoundService : IRoundService
{
    private readonly IRoundRepository _roundRepository;

    public RoundService(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;
    }
    
    public Task CreateRoundAsync(int totalMatches)
    {
        return _roundRepository.CreateRoundAsync(totalMatches);
    }
    
    public Task<Models.Round> GetRoundActiveAsync()
    {
        return _roundRepository.GetRoundActiveAsync();
    }
}
