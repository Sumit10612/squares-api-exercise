namespace SquareApi.Models;

public class Square
{
    public List<Point> Points { get; set; }

    public override string ToString() => string.Join(",", Points.OrderBy(p => p.X).ThenBy(p => p.Y));
}
