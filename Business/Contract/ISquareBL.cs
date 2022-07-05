using SquareApi.Models;

namespace SquareApi.Business.Contract;

public interface ISquareBL
{
    Task<List<Square>> GetAsync();
    Task AddAsync(IEnumerable<Point> points);
    Task DeleteAsync(int id);
}
