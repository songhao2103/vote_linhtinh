using VoteLinhTinh.Models;

namespace VoteLinhTinh.Services;

public interface ISongService
{
    public Task<List<Song>> GetAllSongsAsync(int pageIndex,
                                            int pageSize,
                                            string? searchKey,
                                            bool? isActive);
    public Task<Song?> GetSongByIdAsync(int id);
    public Task<int> UpsertSongsAsync();
}
