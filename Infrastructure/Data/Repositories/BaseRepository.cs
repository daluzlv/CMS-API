using Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data.Repositories;

public class BaseRepository<T>(AppDbContext dbContext) : IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<T>> GetAsync(Expression<Func<T, bool>>? expression)
    {
        var query = _dbContext.Set<T>().AsQueryable();
        if (expression == null)
            return await query.ToListAsync();

        return await query.Where(expression).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbContext.Set<T>().FindAsync(id);
    public async Task<T?> GetByIdAsync(string id) => await _dbContext.Set<T>().FindAsync(id);

    public void Add(T entity) => _dbContext.Add(entity);

    public void Delete(T entity) => _dbContext.Remove(entity);

    public void Update(T entity) => _dbContext.Update(entity);

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken) > 0;
}
