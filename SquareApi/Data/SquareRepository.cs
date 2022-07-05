using Microsoft.EntityFrameworkCore;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Data;

public class SquareRepository : RepositoryBase<Square>, IRepositoryBase<Square>
{
    public SquareRepository(SquareApiDbContext context) : base(context)
    {
    }

    public override Task<List<Square>> GetAllAsync() => dbSet.Include(nameof(Point)).ToListAsync();

    public override async Task DeleteAsync(int pointId)
    {
        var squareIdsToDelete = await dbSet.Where(s => s.Point.Id == pointId)
            .Select(s => s.SquareId)
            .Distinct()
            .ToListAsync();

        if (squareIdsToDelete.Any())
        {
            var squaresToDelete = await dbSet.Where(s => squareIdsToDelete.Contains(s.SquareId)).ToListAsync();
            dbSet.RemoveRange(squaresToDelete);
        }
    }
}
