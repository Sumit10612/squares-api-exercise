using SquareApi.Business.Contract;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Business;

/// <summary>
/// Implementaion of <see cref="ISquareService"/>
/// </summary>
public class SquareService : ISquareService
{
    private HashSet<string> _points;

    private readonly IPointRepository _pointRepository;

    public SquareService(IPointRepository pointRepository) => _pointRepository = pointRepository ??
        throw new ArgumentNullException(nameof(pointRepository));

    /// <summary>
    /// loops through all the points and finds all the squares corresponding to
    /// each point using helper method <see cref="FindPossibleSquares(IEnumerable{Point}, Point)"/>
    /// </summary>
    /// <returns>void</returns>
    public async Task<IEnumerable<Square>> GetSquaresAsync()
    {
        var points = await _pointRepository.GetAllAsync();
        _points = points.Select(p => p.ToString()).ToHashSet();

        var tasks = new List<Task<IEnumerable<Square>>>();
        foreach (var point in points)
        {
            tasks.Add(Task.Run(() => FindPossibleSquares(points, point)));
        }

        var resultSet = await Task.WhenAll(tasks);

        var squares = new List<Square>();

        var squareSet = new HashSet<string>();
        foreach (var result in resultSet)
        {
            foreach (var square in result)
            {
                var s = square.ToString();
                if (!squareSet.Contains(s))
                {
                    squares.Add(square);
                    squareSet.Add(s);
                }
            }
        }

        return squares;
    }

    /// <summary>
    /// Helper method to find all possible squares for the specified Point.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="a"></param>
    /// <returns><see cref="IEnumerable{Square}"/></returns>
    private IEnumerable<Square> FindPossibleSquares(IEnumerable<Point> points, Point a)
    {
        var squares = new List<Square>();

        // Loop through each point & taking `point` as second point of a diagonal
        // find other two points & then check whether those points exists in the
        // list or not.
        foreach (var c in points)
        {
            if (a.Equals(c)) continue;

            var diagVertex = GetRestPoints(a, c);
            if (diagVertex != null && _points.Contains(diagVertex[0].ToString()) 
                && _points.Contains(diagVertex[1].ToString()))
            {
                // if other to point exists means it's a square
                squares.Add(new Square
                {
                    Points = new List<Point> { a, diagVertex[0], c, diagVertex[1] }
                });
            }
        }
        return squares;
    }

    /// <summary>
    /// Method to find other points b and d considering a and c as diagonal point.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    private Point[] GetRestPoints(Point a, Point c)
    {
        // find mid point of point a and c
        var midX = (float)(a.X + c.X) / 2;
        var midY = (float)(a.Y + c.Y) / 2;

        // calculate point b
        var Ax = a.X - midX;
        var Ay = a.Y - midY;
        var bX = midX - Ay;
        var bY = midY + Ax;

        // calculate point d
        var cX = (c.X - midX);
        var cY = (c.Y - midY);
        var dX = midX - cY;
        var dY = midY + cX;

        return IsInteger(bX) && IsInteger(bY) && IsInteger(dX) && IsInteger(dY)
            ? new[] { new Point { X = (int)bX, Y = (int)bY }, new Point { X = (int)dX, Y = (int)dY } }
            : null;
    }

    /// <summary>
    /// Validates whether given folat is integer
    /// </summary>
    /// <param name="p"><see cref="Point"/></param>
    /// <returns></returns>
    private bool IsInteger(float p) => p - (int)p == 0;
}
