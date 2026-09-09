using Microsoft.AspNetCore.Mvc;
using VoteLinhTinh.Services;

namespace VoteLinhTinh.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoundController : ControllerBase
{
    private readonly IRoundService _roundService;

    public RoundController(IRoundService roundService)
    {
        _roundService = roundService;
    }

    [HttpPost("create/{totalMatches}")]
    public async Task<IActionResult> CreateRound([FromRoute] int totalMatches)
    {
        await _roundService.CreateRoundAsync(totalMatches);
        return Ok();
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveRound()
    {
        var round = await _roundService.GetRoundActiveAsync();
        return Ok(round);
    }
}
