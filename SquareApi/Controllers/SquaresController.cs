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
        throw new ArgumentNullException(nameof(ISquareBL));

    /// <summary>
    /// Returns all possible squares.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Square>>> Get() => 
        Ok(await _squareBL.GetAsync());
        
}
