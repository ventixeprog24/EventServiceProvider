﻿using EventServiceProvider.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace EventServiceProvider.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<bool> CreateAsync(TEntity entity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetAllAsync(bool orderByDescending = false, Expression<Func<TEntity, object?>>? sortBy = null, Expression<Func<TEntity, bool>>? filterBy = null, params Expression<Func<TEntity, object?>>[] includes);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object?>>[] includes);
    Task<bool> UpdateAsync(TEntity entity);
}

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly EventDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    protected BaseRepository(EventDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }


    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return false;

        try
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entity = await _dbSet.FirstOrDefaultAsync(expression);
        if (entity == default)
            return false;

        try
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return !await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }


    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(
        bool orderByDescending = false, 
        Expression<Func<TEntity, object?>>? sortBy = null, 
        Expression<Func<TEntity, bool>>? filterBy = null, 
        params Expression<Func<TEntity, object?>>[] includes
        )
    {
        IQueryable<TEntity> query = _dbSet;

        // filter som gör att vi kan hämta alla som är av en viss status (ex. COMPLETED)
        if (filterBy != null)
            query = query.Where(filterBy);

        // inludes inkluderar all olika tabeller som jag vill ha med (ex. .Include(x => x.User)
        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        // sortBy hanterar sorteringen av listan, ASC eller DESC och fält (ex. OrderBy(x => x.Created))
        if (sortBy != null)
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> findBy, params Expression<Func<TEntity, object?>>[] includes)
    {
        IQueryable<TEntity> query = _dbSet;

        if (includes != null && includes.Length != 0)
            foreach (var include in includes)
                query = query.Include(include);

        var entity = await query.FirstOrDefaultAsync(findBy);
        return entity;
    }

    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        if (entity == null)
            return false;

        try
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public Task<bool> AddAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
