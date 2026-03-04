using IpGeoApi.Data;
using IpGeoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IpGeoApi.Repositories;

public class HistoryRepository : IHistoryRepository
{
    private readonly AppDbContext _db;
    public HistoryRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary> Create a new history record </summary>
    /// <param name="history">History record to create</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task CreateAsync(History history)
    {
        _db.Histories.Add(history);
        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Get a paginated list of history records with optional filtering and sorting
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="sortBy"></param>
    /// <param name="desc"></param>
    /// <param name="skip"></param>
    /// <param name="take"></param>
    /// <returns></returns>
    public async Task<IEnumerable<History>> GetListAsync(string? ip, DateTime? from, DateTime? to, string? sortBy, bool desc, int skip, int take)
    {

        var query = _db.Histories.AsQueryable();
        if (!string.IsNullOrEmpty(ip))
            query = query.Where(h => h.Ip == ip);
        if (from.HasValue)
        {
            var fromUtc = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);
            query = query.Where(h => h.CreatedAt >= fromUtc);
        }

        if (to.HasValue)
        {
            // Move to next day at midnight
            var toUtc = DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc);
            query = query.Where(h => h.CreatedAt <= toUtc);
        }
        if (!string.IsNullOrEmpty(sortBy))
        {
            if (sortBy.ToLower() == "ip")
                query = desc ? query.OrderByDescending(h => h.Ip) : query.OrderBy(h => h.Ip);
            else
                query = desc ? query.OrderByDescending(h => h.CreatedAt) : query.OrderBy(h => h.CreatedAt);
        }
        else
        {
            query = query.OrderByDescending(h => h.CreatedAt);
        }
        return await query.Skip(skip).Take(take).ToListAsync();
    }

    /// <summary> Count total history records matching optional filters </summary>
    /// <param name="ip"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>Total count of matching history records</returns>
    public async Task<int> CountAsync(string? ip, DateTime? from, DateTime? to)
    {
        var query = _db.Histories.AsQueryable();
        if (!string.IsNullOrEmpty(ip))
            query = query.Where(h => h.Ip == ip);
        if (from.HasValue)
        {
            var fromUtc = DateTime.SpecifyKind(from.Value, DateTimeKind.Utc);
            query = query.Where(h => h.CreatedAt >= fromUtc);
        }
        if (to.HasValue)
        {
            // Move to next day at midnight
            var toUtc = DateTime.SpecifyKind(to.Value.Date.AddDays(1), DateTimeKind.Utc);
            query = query.Where(h => h.CreatedAt <= toUtc);
        }
        return await query.CountAsync();
    }

    /// <summary> Delete a history record by ID </summary>
    /// <param name="id">ID of the history record to delete</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _db.Histories.FindAsync(id);
        if (entity != null)
        {
            _db.Histories.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }

    /// <summary> Delete multiple history records by their IDs </summary>
    /// <param name="ids">Array of IDs of the history records to delete</param>
    /// <returns>Task representing the asynchronous operation</returns>
    public async Task DeleteBatchAsync(Guid[] ids)
    {
        var entities = await _db.Histories.Where(h => ids.Contains(h.Id)).ToListAsync();
        if (entities.Any())
        {
            _db.Histories.RemoveRange(entities);
            await _db.SaveChangesAsync();
        }
    }
}
