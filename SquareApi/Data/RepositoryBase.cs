using Microsoft.EntityFrameworkCore;
using SquareApi.Data.Contracts;

namespace SquareApi.Data;

/// <summary>
/// Base implementaion of <see cref="IRepositoryBase{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly SquareApiDbContext context;
    protected DbSet<T> dbSet;

    protected RepositoryBase(SquareApiDbContext context)
    {
        this.context = context;
        dbSet = this.context.Set<T>();
    }

    public virtual Task<List<T>> GetAllAsync() => dbSet.ToListAsync();

    public virtual Task AddRangeAsync(IEnumerable<T> values) => dbSet.AddRangeAsync(values);

    public abstract Task DeleteAsync(int id);
}
