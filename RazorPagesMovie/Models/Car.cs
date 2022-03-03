using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RazorPages.Models
{
  public class Car
  {
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Make { get; set; } = string.Empty;

    [StringLength(60, MinimumLength = 3)]
    [Required]
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Doors { get; set; }
    public string Colour { get; set; }= string.Empty;

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
  }
}