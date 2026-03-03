using IpGeoApi.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IpGeoApi.Services;

public class IpInfoService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _token;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IpInfoService(IHttpClientFactory clientFactory, IConfiguration config, IHttpContextAccessor httpContextAccessor)
    {
        _clientFactory = clientFactory;
        _token = Environment.GetEnvironmentVariable("IpGeoToken") ?? config["IpGeoToken"] ?? string.Empty;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IpInfoResponse?> GetIpInfoAsync(string ip)
    {
        var client = _clientFactory.CreateClient("IpInfo");
        var request = new HttpRequestMessage(HttpMethod.Get, $"/{ip}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IpInfoResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<IpInfoResponse?> GetIpInfoMeAsync()
    {
        var ip = _httpContextAccessor.HttpContext?
                .Request
                .Headers["X-Forwarded-For"]
                .FirstOrDefault();
        var realIp = ip.Split(',')[0].Trim();
        var headers = _httpContextAccessor.HttpContext?.Request.Headers;
        Console.WriteLine($"-->Headers: {headers}");
        Console.WriteLine($"-->Client IP: {realIp}");
        var client = _clientFactory.CreateClient("IpInfo");
        // var request = new HttpRequestMessage(HttpMethod.Get, "/");
        var request = new HttpRequestMessage(HttpMethod.Get, $"/{realIp}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        // request.Headers.Add("X-Forwarded-For", _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        // request.Headers.Add("X-Real-IP", _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        // request.Headers.Add("X-Client-IP", _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        // request.Headers.Add("X-Forwarded", _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IpInfoResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
