using SquareApi.Business;
using SquareApi.Business.Contract;

namespace SquareApi.Tests.Business;

[TestClass]
public class PointServiceTests
{
    private readonly IPointService _pointService;

    public PointServiceTests()
    {
        var builder = new DbContextOptionsBuilder<SquareApiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new SquareApiDbContext(builder.Options);
        var repo = new PointRepository(context);
        _pointService = new PointService(repo);
    }

    [TestMethod]
    public async Task AddPointTest()
    {
        await _pointService.AddPointAsync(new Point { X = 0, Y = 0 });

        var count = (await _pointService.GetPointsAsync()).Count();

        Assert.AreEqual(1, count);
    }

    [TestMethod]
    public async Task AddPointsTest()
    {
        await _pointService.AddPointsAsync(new List<Point> 
        { 
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 1 },
        });

        var count = (await _pointService.GetPointsAsync()).Count();

        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public async Task DeletePointTest()
    {
        await _pointService.AddPointsAsync(new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point { X = 0, Y = 1 },
        });

        var count = (await _pointService.GetPointsAsync()).Count();

        Assert.AreEqual(2, count);

        await _pointService.DeletePointAsync(new Point { X = 0, Y = 0 });

        count = (await _pointService.GetPointsAsync()).Count();

        Assert.AreEqual(1, count);
    }
}
