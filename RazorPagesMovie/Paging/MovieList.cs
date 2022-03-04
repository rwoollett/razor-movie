using RazorPages.Models;

namespace RazorPages.Paging
{
  public class MovieList
  {
    public IEnumerable<Movie> movie { get; set; } = new List<Movie>();
    public PagingInfo pagingInfo { get; set; } = new PagingInfo();
  }
}