using IpGeoApi.Models;
using IpGeoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IpInfoController : ControllerBase
{
    private readonly IpInfoService _ipInfoService;
    private readonly HistoryService _historyService;
    public IpInfoController(IpInfoService ipInfoService, HistoryService historyService)
    {
        _ipInfoService = ipInfoService;
        _historyService = historyService;
    }

    [HttpGet("{ip}")]
    public async Task<IActionResult> GetIpInfo(string ip)
    {
        var result = await _ipInfoService.GetIpInfoAsync(ip);
        if (result == null)
            return NotFound();

        // Create history record with geolocation
        var history = new History
        {
            Ip = ip,
            Geolocation = result.Loc ?? string.Empty,
            IpInfoJson = System.Text.Json.JsonSerializer.Serialize(result),
            CreatedAt = DateTime.UtcNow
        };
        await _historyService.CreateAsync(history);

        return Ok(result);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyIpInfo()
    {
        var result = await _ipInfoService.GetIpInfoMeAsync();
        if (result == null)
            return NotFound();

        return Ok(result);
    }
}
