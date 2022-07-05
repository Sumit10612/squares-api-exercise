using Microsoft.EntityFrameworkCore;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Data;

public class PointRepository : RepositoryBase<Point>, IRepositoryBase<Point>
{
    public PointRepository(SquareApiDbContext context) : base(context)
    {
    }

    public override async Task DeleteAsync(int id)
    {
        var pointToRemove = await dbSet.FirstOrDefaultAsync(p => p.Id == id);
        if(pointToRemove != null)
            dbSet.Remove(pointToRemove);
    }
}
