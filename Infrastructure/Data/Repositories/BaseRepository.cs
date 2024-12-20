using System.Linq.Expressions;
using Domain.Interfaces.Repositories.Base;

namespace Infrastructure.Data.Repositories;

public class BaseRepository<T>(AppDbContext dbContext) : IRepository<T> where T : class
{
    private readonly AppDbContext _dbContext = dbContext;

    public IQueryable<T> GetAsync(Expression<Func<T, bool>>? expression)
    {
        var query = _dbContext.Set<T>().AsQueryable();
        if (expression == null)
            return query;

        return query.Where(expression);
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbContext.Set<T>().FindAsync(id);
    public async Task<T?> GetByIdAsync(string id) => await _dbContext.Set<T>().FindAsync(id);

    public void Add(T entity) => _dbContext.Add(entity);

    public void Delete(T entity) => _dbContext.Remove(entity);

    public void Update(T entity) => _dbContext.Update(entity);

    public async Task<bool> CommitAsync(CancellationToken cancellationToken = default) =>
        await _dbContext.SaveChangesAsync(cancellationToken) > 0;
}
