using IpGeoApi.DTOs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IpGeoApi.Services;

public class IpInfoService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _token;

    public IpInfoService(IHttpClientFactory clientFactory, IConfiguration config)
    {
        _clientFactory = clientFactory;
        _token = Environment.GetEnvironmentVariable("IpGeoToken") ?? config["IpGeoToken"] ?? string.Empty;
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
}
