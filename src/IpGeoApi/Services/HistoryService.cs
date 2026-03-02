using IpGeoApi.Models;
using IpGeoApi.Repositories;

namespace IpGeoApi.Services;

public class HistoryService
{
    private readonly IHistoryRepository _repo;
    public HistoryService(IHistoryRepository repo)
    {
        _repo = repo;
    }

    public Task CreateAsync(History history) => _repo.CreateAsync(history);
    public Task<IEnumerable<History>> GetListAsync(string? ip, DateTime? from, DateTime? to, string? sortBy, bool desc, int skip, int take) => _repo.GetListAsync(ip, from, to, sortBy, desc, skip, take);
    public Task<int> CountAsync(string? ip, DateTime? from, DateTime? to) => _repo.CountAsync(ip, from, to);
    public Task DeleteAsync(Guid id) => _repo.DeleteAsync(id);
    public Task DeleteBatchAsync(Guid[] ids) => _repo.DeleteBatchAsync(ids);
}
