using SquareApi.Models;

namespace SquareApi.Business.Contract;

public interface ISquareService
{
    Task<IEnumerable<Square>> GetSquaresAsync();
}
