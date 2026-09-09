using Microsoft.AspNetCore.Mvc;
using VoteLinhTinh.Services;
using VoteLinhTinh.DTOs;

namespace VoteLinhTinh.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpGet("next")]
    public async Task<IActionResult> GetNextMatches([FromQuery] int? currentMatchId)
    {
        var matches = await _matchService.GetNextMatchesAsync(currentMatchId);
        return Ok(matches);
    }

    [HttpPost("create-by-round/{roundId}")]
    public async Task<IActionResult> CreateMatchesByRound([FromRoute] int roundId)
    {
        await _matchService.CreateMatchesByRound(roundId);
        return Ok();
    }

    [HttpPost("complete")]
    public async Task<IActionResult> CompleteMatch([FromQuery] int matchId, [FromQuery] int winnerSongId)
    {
        await _matchService.CompletedMatchAsync(matchId, winnerSongId);
        return Ok();
    }
}
