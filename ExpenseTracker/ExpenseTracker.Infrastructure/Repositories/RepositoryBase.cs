﻿using ExpenseTracker.Domain.Common;
using ExpenseTracker.Domain.Exceptions;
using ExpenseTracker.Domain.Interfaces;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Repositories;

internal abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
{
    protected readonly ExpenseTrackerDbContext _context;

    protected RepositoryBase(ExpenseTrackerDbContext context)
    {
        _context = context;
    }

    public abstract List<TEntity> GetAll(Guid userId);

    public virtual TEntity GetById(int id)
    {
        var entity = GetOrThrow(id);

        return entity;
    }

    public TEntity Create(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Add(entity);

        return entity;
    }

    public void Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(int id)
    {
        var entity = GetOrThrow(id);

        _context.Set<TEntity>().Remove(entity);
    }

    public bool Exists(int id)
        => _context.Set<TEntity>().Any(x => x.Id == id);

    private TEntity GetOrThrow(int id)
    {
        var entity = _context.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefault(x => x.Id == id);

        if (entity is null)
        {
            throw new EntityNotFoundException($"{typeof(TEntity)} with id:{id} is not found");
        }

        return entity;
    }
}
