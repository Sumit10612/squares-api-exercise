using Microsoft.AspNetCore.Mvc;
using SquareApi.Business.Contract;
using SquareApi.Models;

namespace SquareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointsController : ControllerBase
{
    private readonly IPointService _service;

    public PointsController(IPointService service) => _service = service ??
        throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// Imports list of points
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /point/import
    ///     [
    ///         {
    ///             "x": 0,
    ///             "y": 0
    ///         },
    ///         {
    ///             "x": 0,
    ///             "y": 1
    ///         }
    ///      ]
    ///
    /// </remarks>
    /// <param name="points"></param>
    [HttpPost("import")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPoints(IEnumerable<Point> points)
    {
        if (!ModelState.IsValid) return BadRequest();

        await _service.AddPointsAsync(points);

        return Ok();
    }

    /// <summary>
    /// Adds point
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///     
    ///     POST /point
    ///     
    ///         {
    ///             "x": 1,
    ///             "y": 0
    ///         }
    ///     
    /// </remarks>
    /// <param name="point"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddPoint(Point point)
    {
        if(!ModelState.IsValid) return BadRequest();

        await _service.AddPointAsync(point);

        return Created("", point);
    }

    /// <summary>
    /// Deletes a specific point
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /points
    ///         {
    ///             "x": 0,
    ///             "y": 0
    ///         }
    /// </remarks>
    /// <param name="point"></param>
    /// <returns></returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePoint(Point point)
    {
        var result = await _service.DeletePointAsync(point);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}
