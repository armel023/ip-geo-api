using IpGeoApi.Models;

namespace IpGeoApi.Repositories;

public interface IHistoryRepository
{
    Task CreateAsync(History history);
    Task<IEnumerable<History>> GetListAsync(string? ip, DateTime? from, DateTime? to, string? sortBy, bool desc, int skip, int take);
    Task<int> CountAsync(string? ip, DateTime? from, DateTime? to);
    Task DeleteAsync(Guid id);
    Task DeleteBatchAsync(Guid[] ids);
}
