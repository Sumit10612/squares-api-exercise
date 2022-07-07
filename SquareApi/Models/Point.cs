using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SquareApi.Models;

public class Point
{
    [JsonIgnore]
    public int Id { get; set; }
    [Required]
    public int X { get; set; }
    [Required]
    public int Y { get; set; }

    public override bool Equals(object obj) => obj is Point point && point.X == X && point.Y == Y;

    public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();

    public override string ToString() => $"{X},{Y}";
}