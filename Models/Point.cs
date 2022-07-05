using System.ComponentModel.DataAnnotations;

namespace SquareApi.Models;

public class Point
{
    public int Id { get; set; }
    [Required]
    public int X { get; set; }
    [Required]
    public int Y { get; set; }

    public override bool Equals(object obj)
    {
        if(obj == null && !(obj is Point)) return false;
        Point other = (Point)obj;
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
}