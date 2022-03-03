using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.Models;

namespace RazorPages.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [BindProperty]
    public Movie Movie { get; set; } = new Movie();

    public void OnGet()
    {
      Movie = new Movie();
    }

    public void OnPostRawFormData() 
    {
      Console.WriteLine("Raw ");
      Console.WriteLine(Movie.Title);

    }
    public void OnPostIndividualFields(Movie movie) 
    {
      Console.WriteLine("Hey ");
      Console.WriteLine(movie.Title);
    }
}
