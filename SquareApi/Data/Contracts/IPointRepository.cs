using SquareApi.Models;

namespace SquareApi.Data.Contracts;

public interface IPointRepository
{
    Task<IEnumerable<Point>> GetAllAsync();
    Task AddRangeAsync(IEnumerable<Point> points);
    Task AddAsync(Point point);
    Task<Point> DeleteAsync(Point point);
}