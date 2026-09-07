using VoteLinhTinh.Models;

namespace VoteLinhTinh.Repositories;

public interface ISongRepository
{
    public Task<List<Song>> GetAllSongsAsync(int pageIndex,
                                            int pageSize,
                                            string? searchKey,
                                            bool? isActive);
    public Task<Song?> GetSongByIdAsync(int id);
    public Task<int> UpsertSongsAsync(List<Song> songs);
}
