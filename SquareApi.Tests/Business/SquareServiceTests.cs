using SquareApi.Business;
using SquareApi.Business.Contract;
using SquareApi.Data.Contracts;
using System.Diagnostics;

namespace SquareApi.Tests.Business;

[TestClass]
public class SquareServiceTests
{
    private readonly IPointRepository _repo;
    private readonly ISquareService _service;

    public SquareServiceTests()
    {
        var builder = new DbContextOptionsBuilder<SquareApiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new SquareApiDbContext(builder.Options);
        _repo = new PointRepository(context);
        _service = new SquareService(_repo);
    }

    [TestMethod]
    [DataRow(1, 0)]
    [DataRow(2, 0)]
    [DataRow(3, 1)]
    [DataRow(4, 2)]
    public async Task GetSquaresTests(int pointIndex, int expected)
    {
        await _repo.AddRangeAsync(GetPoint(pointIndex));

        var actual = (await _service.GetSquaresAsync())?.Count();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow(5000)]
    public async Task FindSquares_Performance_Tests(int noOfPoints)
    {
        await _repo.AddRangeAsync(GenerateRandomPoints(noOfPoints));

        var sw = new Stopwatch();
        sw.Start();
        await _service.GetSquaresAsync();
        sw.Stop();

        Assert.IsTrue(sw.ElapsedMilliseconds < 5_000);
    }

    private List<Point> GetPoint(int index) => index switch
    {
        1 => new List<Point> { new Point { X = 0, Y = 0 } },
        2 => new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point{ X = 1, Y = 1 },
            new Point{ X = 1, Y = 0 },
        },
        3 => new List<Point>
        {
            new Point { X = 0, Y = 0 },
            new Point{ X = 1, Y = 1 },
            new Point{ X = 1, Y = 0 },
            new Point{ X = 0, Y = 1 },
            new Point{ X = 2, Y = 1 },
        },
        4 => new List<Point> 
        { 
            new Point{ X = 0, Y = 0 },
            new Point{ X = 0, Y = 1 },
            new Point{ X = 1, Y = 0 },
            new Point{ X = 1, Y = 1 },
            new Point{ X = 2, Y = 0 },
            new Point{ X = 2, Y = 1 }
        },
        _ => new List<Point>()
    };

    private IEnumerable<Point> GenerateRandomPoints(int noOfPoints)
    {
        var randomPoints = new List<Point>();
        var random = new Random();
        while (noOfPoints > 0)
        {
            var point = new Point
            {
                X = random.Next(-1000, 1000),
                Y = random.Next(-1000, 1000),
            };
            if (!randomPoints.Contains(point))
            {
                randomPoints.Add(point);
                noOfPoints--;
            }
        }
        return randomPoints;
    }
}
