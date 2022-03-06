#nullable disable
using System.ComponentModel.DataAnnotations;

namespace RazorPages.Models{
  public class Tag 
  {
    public int ID { get; set; }

    [StringLength(50, MinimumLength = 3)]
    [Required]    
    public string Name { get; set; } = string.Empty;

    public Movie Movie { get; set; }
  }
}