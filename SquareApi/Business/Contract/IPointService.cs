using SquareApi.Models;

namespace SquareApi.Business.Contract;

public interface IPointService
{
    Task<IEnumerable<Point>> GetPointsAsync();
    Task AddPointsAsync(IEnumerable<Point> points);
    Task AddPointAsync(Point point);
    Task<Point> DeletePointAsync(Point point);
}
