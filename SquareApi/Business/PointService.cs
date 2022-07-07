using SquareApi.Business.Contract;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Business;

public class PointService : IPointService
{
    private readonly IPointRepository _repository;

    public PointService(IPointRepository repository) => _repository = repository ??
        throw new System.ArgumentNullException(nameof(repository));

    public Task AddPointAsync(Point point) => _repository.AddAsync(point);

    public Task AddPointsAsync(IEnumerable<Point> points) => _repository.AddRangeAsync(points);

    public Task<Point> DeletePointAsync(Point point) => _repository.DeleteAsync(point);

    public Task<IEnumerable<Point>> GetPointsAsync() => _repository.GetAllAsync();
}
