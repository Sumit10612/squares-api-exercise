using Microsoft.AspNetCore.Mvc;
using SquareApi.Business.Contract;
using SquareApi.Models;

namespace SquareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SquareController : ControllerBase
{
    private readonly ISquareService _service;

    public SquareController(ISquareService service) => _service = service ??
        throw new ArgumentNullException(nameof(service));

    /// <summary>
    /// Returns all possible squares with all 4 points forming that square.
    /// </summary>
    /// <remarks>
    /// Sample response:
    /// 
    ///     GET: /squares
    ///         [
    ///             {
    ///                 "points": [
    ///                   {
    ///                     "x": 0,
    ///                     "y": 0
    ///                   },
    ///                   {
    ///                     "x": 1,
    ///                     "y": 0
    ///                   },
    ///                   {
    ///                     "x": 1,
    ///                     "y": 1
    ///                   },
    ///                   {
    ///                     "x": 0,
    ///                     "y": 1
    ///                   }
    ///                 ]
    ///             }
    ///         ]
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Square>>> GetSquares()
    {
        var squares = await _service.GetSquaresAsync();
        if(squares.Any())
            return Ok(squares);

        return NotFound();
    }

}
