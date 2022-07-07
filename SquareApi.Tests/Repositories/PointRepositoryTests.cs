using Microsoft.EntityFrameworkCore;
using SquareApi.Data;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Tests.Repositories;

[TestClass]
public class PointRepositoryTests
{
    private readonly IPointRepository _repository;
    private List<Point> _points => new List<Point>
    {
        new Point { X = 0, Y = 0 },
        new Point { X = 0, Y = 1 },
        new Point { X = 1, Y = 0 },
        new Point { X = 1, Y = 1 }
    };

    public PointRepositoryTests()
    {
        var builder = new DbContextOptionsBuilder<SquareApiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new SquareApiDbContext(builder.Options);
        _repository = new PointRepository(context);
    }

    [TestMethod]
    public async Task AddPointTest()
    {
        await _repository.AddRangeAsync(_points);

        int count = (await _repository.GetAllAsync()).Count();

        Assert.AreEqual(4, count);
    }

    [TestMethod]
    public async Task DeletePointTest()
    {
        await _repository.AddRangeAsync(_points);

        var deletedPoint = await _repository.DeleteAsync(_points[0]);

        int count = (await _repository.GetAllAsync()).Count();

        Assert.IsTrue(_points[0].Equals(deletedPoint));
        Assert.AreEqual(3, count);
    }
}
