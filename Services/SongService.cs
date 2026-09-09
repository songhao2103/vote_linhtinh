using VoteLinhTinh.DTOs;
using VoteLinhTinh.Models;
using VoteLinhTinh.Repositories;

namespace VoteLinhTinh.Services;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;
    private readonly HttpClient _httpClient;
    private readonly string _externalApiUrl1 = "https://api.uwufufu.com/v1/selections?page=1&perPage=1000&worldcupId=170440";
    private readonly string _externalApiUrl2 = "https://api.uwufufu.com/v1/selections?page=1&perPage=1000&worldcupId=168808";

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
        List<ExternalSongDTO> externalSongs1 = [];
        List<ExternalSongDTO> externalSongs2 = [];

        try
        {
            var response = await _httpClient.GetAsync(_externalApiUrl1);

            var result = await response.Content
                .ReadFromJsonAsync<ExternalSongResponse>();

            externalSongs1 = result?.Data ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Có lỗi khi gọi API lấy danh sách nhạc 1: {ex.Message}"
            );
        }

        try
        {
            var response = await _httpClient.GetAsync(_externalApiUrl2);

            var result = await response.Content
                .ReadFromJsonAsync<ExternalSongResponse>();

            externalSongs2 = result?.Data ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Có lỗi khi gọi API lấy danh sách nhạc 2: {ex.Message}"
            );
        }

        var songs = externalSongs1
            .Concat(externalSongs2)
            .Where(dto =>
                !string.IsNullOrWhiteSpace(dto.Name) &&
                !string.IsNullOrWhiteSpace(dto.VideoUrl)
            )
            .Select(dto => new Song
            {
                Name = dto.Name!,
                VideoUrl = dto.VideoUrl!,
                ResourceUrl = dto.ResourceUrl,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            })
            .DistinctBy(x => new
            {
                x.Name,
                x.VideoUrl
            })
            .ToList();

        return await _songRepository.UpsertSongsAsync(songs);
    }
}
