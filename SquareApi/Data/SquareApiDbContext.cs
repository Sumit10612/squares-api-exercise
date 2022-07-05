using Microsoft.EntityFrameworkCore;
using SquareApi.Models;

namespace SquareApi.Data;

public class SquareApiDbContext : DbContext
{
    public DbSet<Point> Points { get; set; }
    public DbSet<Square> Squares { get; set; }

    public SquareApiDbContext(DbContextOptions<SquareApiDbContext> options) : base(options)
    {

    }
}
