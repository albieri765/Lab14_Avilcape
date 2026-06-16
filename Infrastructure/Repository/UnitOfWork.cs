using System.Collections;
using Domain.Repository;
using Infrastructure.Context;

namespace Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Hashtable   _repositories;

    public UnitOfWork(AppDbContext context)
    {
        _context      = context;
        _repositories = new Hashtable();
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;
        if (_repositories.ContainsKey(type))
            return (IRepository<T>)_repositories[type]!;

        var instance = new Repository<T>(_context);
        _repositories.Add(type, instance);
        return instance;
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    public void Dispose()                      => _context.Dispose();
}