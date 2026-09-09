using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public interface IRoundRepository
{
    public Task<Round> GetRoundActiveAsync();
    public Task CreateRoundAsync(int totalMatches);
    public Task DeactivateRoundAsync(int roundId);
    public Task<List<Round>> GetAllRoundsAsync();  
}
