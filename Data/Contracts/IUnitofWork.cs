using SquareApi.Models;

namespace SquareApi.Data.Contracts;

public interface IUnitofWork
{
    IRepositoryBase<Point> Points { get; }
    IRepositoryBase<Square> Squares { get; }
    Task SaveChangesAsync();
}
