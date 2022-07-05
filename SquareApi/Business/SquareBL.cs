using SquareApi.Business.Contract;
using SquareApi.Data.Contracts;
using SquareApi.Models;

namespace SquareApi.Business;

/// <summary>
/// Implementaion of <see cref="ISquareBL"/>
/// </summary>
public class SquareBL : ISquareBL
{
    private List<Point> _points;
    private readonly IUnitofWork _unitofWork;

    public SquareBL(IUnitofWork unitofWork) => _unitofWork = unitofWork ??
        throw new ArgumentNullException(nameof(IUnitofWork));

    /// <summary>
    /// Accepts <see cref="IEnumerable"/> of <see cref="Point"/>
    /// loops through all the points and finds all the squares corresponding to
    /// each point using helper method <see cref="FindPossibleSquares(Point)"/>
    /// and then commits <see cref="Point"/> & <see cref="Square"/>
    /// </summary>
    /// <param name="points"></param>
    /// <returns>void</returns>
    public async Task AddAsync(IEnumerable<Point> points)
    {
        var pointsToInsert = new List<Point>();
        _points = await _unitofWork.Points.GetAllAsync();
        foreach (var point in points)
        {
            if (!_points.Contains(point)) pointsToInsert.Add(point);
        }
        _points.AddRange(pointsToInsert);
        if (pointsToInsert.Any())
        {
            await _unitofWork.Points.AddRangeAsync(pointsToInsert);

            var tasks = new List<Task<IEnumerable<Square>>>();
            foreach (var point in pointsToInsert)
            {
                tasks.Add(Task.Run(() => FindPossibleSquares(point)));
            }
            await Task.WhenAll(tasks);

            var sqaresToInsert = new List<Square>();
            foreach (var task in tasks)
            {
                var sqares = await task;
                var squareGroup = sqares.GroupBy(x => x.SquareId);
                foreach (var sg in squareGroup)
                {
                    var sgp = sg.Select(p => p.Point);
                    var stiCount = sqaresToInsert.Where(s => sgp.Contains(s.Point))
                        .GroupBy(s => s.Id)
                        .Select(s => s.Count())
                        .ToList();
                    if (!stiCount.Any(c => c >= 4))
                    {
                        sqaresToInsert.AddRange(sg.ToList());
                    }
                }
            }

            if (sqaresToInsert.Any()) await _unitofWork.Squares.AddRangeAsync(sqaresToInsert);

            await _unitofWork.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Helper method to find all possible squares for the specified Point.
    /// </summary>
    /// <param name="a"></param>
    /// <returns><see cref="IEnumerable{Square}"/></returns>
    private IEnumerable<Square> FindPossibleSquares(Point a)
    {
        var squares = new List<Square>();

        // Loop through each point & taking `point` as second point of a diagonal
        // find other two points & then check whether those points exists in the
        // list or not.
        // if exists compare sides for equality.
        foreach (var c in _points)
        {
            if (!a.Equals(c))
            {
                var b = new Point { X = c.X, Y = a.Y };
                var d = new Point { X = a.X, Y = c.Y };

                if (Math.Abs(a.X - b.X) == Math.Abs(a.Y - c.Y) && Math.Abs(d.X - c.X) == Math.Abs(b.Y - c.Y))
                {
                    var pointInList = _points.Where(p => new Point[] { b, d }.Contains(p)).ToList();
                    if (pointInList.Count() == 2)
                    {
                        var id = Guid.NewGuid().ToString();
                        squares.Add(new Square { SquareId = id, Point = a });
                        squares.Add(new Square { SquareId = id, Point = c });
                        squares.Add(new Square { SquareId = id, Point = pointInList[0] });
                        squares.Add(new Square { SquareId = id, Point = pointInList[1] });
                    }
                }
            }
        }
        return squares;
    }


    /// <summary>
    /// Deletes all squares corresponding to specified Point 
    /// & deletes the point from points table
    /// </summary>
    /// <param name="id"></param>
    /// <returns>void</returns>
    public async Task DeleteAsync(int id)
    {
        await _unitofWork.Squares.DeleteAsync(id);
        await _unitofWork.Points.DeleteAsync(id);

        await _unitofWork.SaveChangesAsync();
    }

    /// <summary>
    /// Get all possible squares.
    /// </summary>
    /// <returns>List<see cref="Square"/></returns>
    public Task<List<Square>> GetAsync() => _unitofWork.Squares.GetAllAsync();
}
