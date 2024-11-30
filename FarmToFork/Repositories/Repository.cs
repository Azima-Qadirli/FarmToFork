using FarmToFork.Context;
using FarmToFork.Exceptions;
using FarmToFork.Models.BaseEntities;
using FarmToFork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FarmToFork.Repositories;

public class Repository<T>:IRepository<T> where T:BaseEntity
{
    private readonly FarmToForkDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(FarmToForkDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async  Task<bool> AddAsync(T entity)
    {
       EntityEntry entityEntry = await _dbSet.AddAsync(entity);
       return entityEntry.State == EntityState.Added;
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
        =>await _dbSet.AddRangeAsync(entities);
    

    public bool Update(T entity)
    {
        EntityEntry entityEntry = _dbSet.Update(entity);
        return entityEntry.State == EntityState.Modified;
    }

    public void UpdateRange(IEnumerable<T> entities)
    => _dbSet.UpdateRange(entities);

    public bool Remove(T entity)
    {
        EntityEntry entityEntry = _dbSet.Update(entity);
        return entityEntry.State == EntityState.Deleted;
    }

    public void RemoveRange(IEnumerable<T> entities)
    => _dbSet.RemoveRange(entities);
    

    public async Task<bool> RemoveAsync(int id)
    {
        var entity = await GetAsync(id);
        EntityEntry entityEntry = _dbSet.Remove(entity);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<T> GetAsync(int id)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
        if(entity == null)
            throw new EntityNotFoundException();
        return entity;
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet;
    }

    public int Save()
    => _context.SaveChanges();

    public async Task<int> SaveAsync()
    =>await _context.SaveChangesAsync();
}