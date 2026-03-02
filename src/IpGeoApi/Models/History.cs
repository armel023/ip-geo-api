using System.ComponentModel.DataAnnotations;

namespace IpGeoApi.Models;

public class History
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Ip { get; set; } = string.Empty;
    [Required]
    public string Geolocation { get; set; } = string.Empty;
    [Required]
    public string IpInfoJson { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
