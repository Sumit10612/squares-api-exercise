using Microsoft.EntityFrameworkCore;
using SquareApi.Business;
using SquareApi.Business.Contract;
using SquareApi.Data;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Tests.Business;

[TestClass]
public class SquareBLTests
{
    private readonly ISquareBL _squareBL;
    private readonly IUnitofWork _uow;

    public SquareBLTests()
    {
        var builder = new DbContextOptionsBuilder<SquareApiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new SquareApiDbContext(builder.Options);
        _uow = new UnitOfWork(context);
        _squareBL = new SquareBL(_uow);
    }

    [TestMethod]
    [DataRow(1, 1, 0)]
    [DataRow(2, 3, 0)]
    [DataRow(3, 5, 1)]
    [DataRow(4, 6, 2)]
    public async Task AddAsync_Test(
        int pointIndex,
        int expectedPointCount,
        int expectedSquareCount)
    {
        await _squareBL.AddAsync(GetPoint(pointIndex));

        var pointCount = (await _uow.Points.GetAllAsync()).Count();
        var squareCount = (await _uow.Squares.GetAllAsync())
            .Select(x => x.SquareId).Distinct().Count();

        Assert.AreEqual(expectedPointCount, pointCount);
        Assert.AreEqual(expectedSquareCount, squareCount);
    }

    [TestMethod]
    public async Task DeleteAsyncTest()
    {
        // adding two squares
        await _squareBL.AddAsync(GetPoint(4));

        // delete point 2 [0, 1]
        await _squareBL.DeleteAsync(2);

        int squaresCount = (await _squareBL.GetAsync()).Select(x => x.SquareId)
            .Distinct().Count();

        Assert.AreEqual(1, squaresCount);

        // delete point 3 [1, 0]
        await _squareBL.DeleteAsync(3);

        squaresCount = (await _squareBL.GetAsync()).Select(x => x.SquareId)
            .Distinct().Count();

        Assert.AreEqual(0, squaresCount);
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
}
