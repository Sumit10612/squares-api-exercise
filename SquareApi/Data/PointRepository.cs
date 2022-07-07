using Microsoft.EntityFrameworkCore;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Data;

public class PointRepository : IPointRepository
{
    private readonly SquareApiDbContext _context;

    public PointRepository(SquareApiDbContext context) => _context = context ??
        throw new ArgumentNullException(nameof(context));

    /// <summary>
    /// Add point to database.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public async Task AddAsync(Point point)
    {
        await _context.AddAsync(point);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Adds <see cref="IEnumerable{Point}"/> points to database.
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    public async Task AddRangeAsync(IEnumerable<Point> points)
    {
        await _context.AddRangeAsync(points);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes specified point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public async Task<Point> DeleteAsync(Point point)
    {
        var pointToDelete = await _context.Points.
            FirstOrDefaultAsync(p => p.X == point.X && p.Y == point.Y);
        if (pointToDelete != null)
        {
            _context.Points.Remove(pointToDelete);
            await _context.SaveChangesAsync();
        }
        return pointToDelete;
    }

    /// <summary>
    /// Gets all points from Database
    /// </summary>
    /// <returns><see cref="IEnumerable{Point}"/></returns>
    public async Task<IEnumerable<Point>> GetAllAsync() => await _context.Points.ToListAsync();
}
