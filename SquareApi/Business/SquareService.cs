using SquareApi.Business.Contract;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Business;

/// <summary>
/// Implementaion of <see cref="ISquareService"/>
/// </summary>
public class SquareService : ISquareService
{
    private const int _processSize = 500;
    private HashSet<string> _pointsSet;
    private List<Point> _points;

    private readonly IPointRepository _pointRepository;

    public SquareService(IPointRepository pointRepository) => _pointRepository = pointRepository ??
        throw new ArgumentNullException(nameof(pointRepository));

    /// <summary>
    /// loops through all the points and finds all the squares corresponding to
    /// each point using helper method <see cref="FindPossibleSquares(int, int)"/>
    /// </summary>
    /// <returns>void</returns>
    public async Task<IEnumerable<Square>> GetSquaresAsync()
    {
        _points = (await _pointRepository.GetAllAsync()).ToList();
        _pointsSet = _points.Select(p => p.ToString()).ToHashSet();

        //process points in parallel with a set of 500
        int processes = (_points.Count / _processSize) + 1;

        var resultSet = new List<IEnumerable<Square>>();
        Parallel.For(0, processes, process => resultSet.Add(FindPossibleSquares(process, _processSize)));


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
    /// <param name="process"></param>
    /// <param name="processSize"></param>
    /// <returns><see cref="IEnumerable{Square}"/></returns>
    private IEnumerable<Square> FindPossibleSquares(int process, int processSize)
    {
        var squares = new List<Square>();

        // Loop through each point & taking `point` as second point of a diagonal
        // find other two points & then check whether those points exists in the
        // list or not.
        var startIndex = process * processSize;
        var endIndex = startIndex + processSize > _points.Count ? _points.Count : startIndex + processSize;
        for (int i = startIndex; i < endIndex; i++)
        {
            for ( int j = 0; j < _points.Count; j++)
            {
                if (_points[i].Equals(_points[j])) continue;

                var diagVertex = GetRestPoints(_points[i], _points[j]);
                if (diagVertex != null && _pointsSet.Contains(diagVertex[0].ToString())
                    && _pointsSet.Contains(diagVertex[1].ToString()))
                {
                    // if other to point exists means it's a square
                    squares.Add(new Square
                    {
                        Points = new List<Point> { _points[i], diagVertex[0], _points[j], diagVertex[1] }
                    });
                }
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
