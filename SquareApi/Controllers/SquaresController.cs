using Microsoft.AspNetCore.Mvc;
using SquareApi.Business.Contract;
using SquareApi.Models;

namespace SquareApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SquaresController : ControllerBase
{
    private readonly ISquareBL _squareBL;

    public SquaresController(ISquareBL squareBL) => _squareBL = squareBL ??
        throw new ArgumentNullException(nameof(squareBL));

    /// <summary>
    /// Returns all possible squares with all 4 points forming that square.
    /// </summary>
    /// <remarks>
    /// Sample response:
    /// 
    ///     GET: /squares
    ///         [
    ///             {
    ///               "squareId": "2cd6ff2b-ce79-45a2-b300-d5a71f35f8d1",
    ///               "point": 
    ///               {
    ///                 "id": 5093,
    ///                 "x": 0,
    ///                 "y": 0
    ///               }
    ///             },
    ///             {
    ///               "squareId": "2cd6ff2b-ce79-45a2-b300-d5a71f35f8d1",
    ///               "point": 
    ///               {
    ///                 "id": 5096,
    ///                 "x": 1,
    ///                 "y": 1
    ///               }
    ///             },
    ///             {
    ///               "squareId": "2cd6ff2b-ce79-45a2-b300-d5a71f35f8d1",
    ///               "point": 
    ///               {
    ///                   "id": 5094,
    ///                 "x": 0,
    ///                 "y": 1
    ///               }
    ///             },
    ///             {
    ///               "squareId": "2cd6ff2b-ce79-45a2-b300-d5a71f35f8d1",
    ///               "point": 
    ///               {
    ///                   "id": 5095,
    ///                   "x": 1,
    ///                   "y": 0
    ///               }
    ///             }
    ///         ]
    /// </remarks>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Square>>> GetSquares() => 
        Ok(await _squareBL.GetAsync());
        
}
