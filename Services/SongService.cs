using VoteLinhTinh.DTOs;
using VoteLinhTinh.Models;
using VoteLinhTinh.Repositories;

namespace VoteLinhTinh.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly HttpClient _httpClient;
    private readonly string _externalApiUrl1 = "https://api.example.com/songs";
    private readonly string _externalApiUrl2 = "https://api.example.com/other-songs";

    public SongService(ISongRepository songRepository, HttpClient httpClient)
    {
        _songRepository = songRepository;
        _httpClient = httpClient;
    }

    public async Task<List<Song>> GetAllSongsAsync(int pageIndex, int pageSize, string? searchKey, bool? isActive)
    {
        return await _songRepository.GetAllSongsAsync(pageIndex, pageSize, searchKey, isActive);
    }

    public async Task<Song?> GetSongByIdAsync(int id)
    {
        return await _songRepository.GetSongByIdAsync(id);
    }
    public async Task<int> UpsertSongsAsync()
    {
        List<ExternalSongDTO> externalSongs1 = new List<ExternalSongDTO>();
        List<ExternalSongDTO> externalSongs2 = new List<ExternalSongDTO>();
        try
        {
            var externalSongs1Response = await _httpClient.GetAsync(_externalApiUrl1);
            externalSongs1 = await externalSongs1Response.Content.ReadFromJsonAsync<List<ExternalSongDTO>>() ?? new List<ExternalSongDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Có lỗi khi gọi API lấy danh sách nhạc 1 : {ex.Message}");
        }

        try
        {
            var externalSongs2Response = await _httpClient.GetAsync(_externalApiUrl2);
            externalSongs2 = await externalSongs2Response.Content.ReadFromJsonAsync<List<ExternalSongDTO>>() ?? new List<ExternalSongDTO>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Có lỗi khi gọi API lấy danh sách nhạc 2 : {ex.Message}");
        }

        var songs = externalSongs1.Concat(externalSongs2).Select(dto => new Song
        {
            Name = dto.Name,
            VideoUrl = dto.VideoUrl,
            ResourceUrl = dto.ResourceUrl,
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        return await _songRepository.UpsertSongsAsync(songs);
    }
}
