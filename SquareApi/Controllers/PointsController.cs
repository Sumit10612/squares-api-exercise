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
    /// Adds list of points
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /points
    ///     [
    ///         {
    ///             "x": 0,
    ///             "y": 0
    ///         },
    ///         {
    ///             "x": 0,
    ///             "y": 1
    ///         },
    ///         {
    ///             "x": 1,
    ///             "y": 0
    ///         },
    ///         {
    ///             "x": 1,
    ///             "y": 1
    ///         }
    ///      ]
    ///
    /// </remarks>
    /// <param name="points"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <response code="200">returns 200 on success</response>
    /// <response code="400">if input is null or with some invalid values</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Post(IEnumerable<Point> points)
    {
        if (!ModelState.IsValid) return BadRequest();
        await _squareBL.AddAsync(points);
        return Ok(points);
    }

    /// <summary>
    /// Deletes a specific point.
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _squareBL.DeleteAsync(id);
        return Ok();
    }
}
