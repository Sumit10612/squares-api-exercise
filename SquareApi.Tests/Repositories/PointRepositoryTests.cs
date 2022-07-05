using Microsoft.EntityFrameworkCore;
using SquareApi.Data;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Tests.Repositories;

[TestClass]
public class PointRepositoryTests
{
    private readonly IUnitofWork _uow;
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
        _uow = new UnitOfWork(context);
    }

    [TestMethod]
    public async Task AddPointTest()
    {
        await _uow.Points.AddRangeAsync(_points);
        await _uow.SaveChangesAsync();

        int count = (await _uow.Points.GetAllAsync()).Count();

        Assert.AreEqual(4, count);
    }

    [TestMethod]
    public async Task DeletePointTest()
    {
        await _uow.Points.AddRangeAsync(_points);
        await _uow.SaveChangesAsync();

        await _uow.Points.DeleteAsync(2);
        await _uow.SaveChangesAsync();

        int count = (await _uow.Points.GetAllAsync()).Count();

        Assert.AreEqual(3, count);
    }
}
