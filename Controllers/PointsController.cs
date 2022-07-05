using Microsoft.AspNetCore.Mvc;
using SquareApi.Business.Contract;
using SquareApi.Models;

namespace SquareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointsController : ControllerBase
{
    private readonly ISquareBL _squareBL;

    public PointsController(ISquareBL squareBL) => _squareBL = squareBL ??
        throw new ArgumentNullException(nameof(squareBL));

    /// <summary>
    /// Endpoint for importing list of points
    /// </summary>
    /// <param name="points"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post(IEnumerable<Point> points)
    {
        if (!ModelState.IsValid) return BadRequest();
        await _squareBL.AddAsync(points);
        return Ok();
    }

    /// <summary>
    /// Endpoint for deleting a point from list.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _squareBL.DeleteAsync(id);
        return Ok();
    }
}
