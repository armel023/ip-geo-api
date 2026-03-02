
using AutoMapper;
using IpGeoApi.DTOs;
using IpGeoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly HistoryService _historyService;
    private readonly IMapper _mapper;
    public HistoryController(HistoryService historyService, IMapper mapper)
    {
        _historyService = historyService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<HistoryListResponse>> GetList([FromQuery] string? ip, [FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? sortBy, [FromQuery] bool desc = false, [FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var items = await _historyService.GetListAsync(ip, from, to, sortBy, desc, skip, take);
        var total = await _historyService.CountAsync(ip, from, to);
        var mapped = _mapper.Map<IEnumerable<HistoryDto>>(items);
        return Ok(new HistoryListResponse { Total = total, Items = mapped });
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] Guid[] ids)
    {
        await _historyService.DeleteBatchAsync(ids);
        return NoContent();
    }
}
