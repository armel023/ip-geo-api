using IpGeoApi.Models;

namespace IpGeoApi.DTOs;

public class HistoryListResponse
{
    public int Total { get; set; }
    public IEnumerable<HistoryDto> Items { get; set; } = new List<HistoryDto>();
}
