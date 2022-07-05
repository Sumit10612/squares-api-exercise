namespace SquareApi.Data.Contracts;

/// <summary>
/// Base interface for all the repositories.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepositoryBase<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task AddRangeAsync(IEnumerable<T> values);
    Task DeleteAsync(int id);
}
