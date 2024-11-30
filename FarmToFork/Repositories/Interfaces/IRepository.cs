using FarmToFork.Models.BaseEntities;

namespace FarmToFork.Repositories.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    bool Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    bool Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    Task<bool>RemoveAsync(int id);
    Task<T>GetAsync(int id);
    IQueryable<T> GetAll();
    int Save();
    Task<int> SaveAsync();
}