namespace VoteLinhTinh.Services
{
    public interface IRoundService
    {
        public Task CreateRoundAsync(int totalMatches);
        public Task<Models.Round> GetRoundActiveAsync();
    }
}
