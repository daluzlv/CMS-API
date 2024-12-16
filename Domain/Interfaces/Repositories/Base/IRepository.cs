using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Base;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAsync(Expression<Func<T, bool>>? expression);
    Task<T?> GetByIdAsync(Guid id);
    Task<T?> GetByIdAsync(string id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> CommitAsync(CancellationToken cancellationToken = default);
}
