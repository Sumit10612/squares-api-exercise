using Microsoft.EntityFrameworkCore;
using SquareApi.Data;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Tests.Repositories;

[TestClass]
public class SquareRepositoryTests
{
    private readonly IUnitofWork _uow;

    public SquareRepositoryTests()
    {
        var builder = new DbContextOptionsBuilder<SquareApiDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString());
        var context = new SquareApiDbContext(builder.Options);
        _uow = new UnitOfWork(context);
    }


    [TestMethod]
    public async Task AddSquaresTest()
    {
        await _uow.Squares.AddRangeAsync(GetSquares());
        await _uow.SaveChangesAsync();

        int count = (await _uow.Squares.GetAllAsync()).Select(x => x.SquareId)
            .Distinct().Count();

        Assert.AreEqual(2, count);
    }

    [TestMethod]
    public async Task DeletePointTest()
    {
        await _uow.Squares.AddRangeAsync(GetSquares());
        await _uow.SaveChangesAsync();

        await _uow.Squares.DeleteAsync(2);
        await _uow.SaveChangesAsync();

        int count = (await _uow.Squares.GetAllAsync()).Select(x => x.SquareId)
            .Distinct().Count();

        Assert.AreEqual(1, count);
    }

    private List<Square> GetSquares()
    {
        var squares = new List<Square>();
        var sid = Guid.NewGuid().ToString();
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 0, Y = 0 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 0, Y = 1 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 1, Y = 0 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 1, Y = 1 },
        });

        sid = Guid.NewGuid().ToString();
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 0, Y = 1 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 1, Y = 1 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 2, Y = 0 },
        });
        squares.Add(new Square
        {
            SquareId = sid,
            Point = new Point { X = 2, Y = 1 },
        });

        return squares;
    }
}
