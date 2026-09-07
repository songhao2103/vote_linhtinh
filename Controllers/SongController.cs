using Microsoft.AspNetCore.Mvc;
using VoteLinhTinh.Services;

namespace VoteLinhTinh.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongService _songService;

    public SongController(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet()]
    public IActionResult GetAllSongAsync([FromQuery] int pageIndex = 1,
                                        [FromQuery] int pageSize = 10,
                                        [FromQuery] string? searchKey = null,
                                        [FromQuery] bool? isActive = null)
    {
        var songs = _songService.GetAllSongsAsync(pageIndex, pageSize, searchKey, isActive).Result;
        return Ok(songs);
    }

    [HttpGet("{id}")]
    public IActionResult GetSongByIdAsync([FromRoute] int id)
    {
        var song = _songService.GetSongByIdAsync(id).Result;
        if (song == null)
        {
            return NotFound();
        }   
        return Ok(song);
    }

    [HttpPost("upsert")]
    public IActionResult UpsertSongsAsync()
    {
        var result = _songService.UpsertSongsAsync().Result;
        return Ok(result);
    }
}
