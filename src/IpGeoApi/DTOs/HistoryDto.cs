using System;

namespace IpGeoApi.DTOs;

public class HistoryDto
{
    public Guid Id { get; set; }
    public string Ip { get; set; } = string.Empty;
    public string Geolocation { get; set; } = string.Empty;
    public string IpInfoJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
