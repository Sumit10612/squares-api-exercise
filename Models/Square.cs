using System.Text.Json.Serialization;

namespace SquareApi.Models;

public class Square
{
    [JsonIgnore]
    public int Id { get; set; }
    public string SquareId { get; set; } 
    public Point Point { get; set; }
}
