using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Services;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages;

public class AboutModel : PageModel
{
  private ICarService _carService;

  public AboutModel(ICarService carService)
  {
    _carService = carService;
  }

  public List<Car> Cars { get; set; } = Array.Empty<Car>().ToList<Car>();

  public string? Msg { get; set; }

  public IActionResult OnGet()
  {
    Msg = "The about page.";
    return Page();
  }

  public PartialViewResult OnGetCarPartial()
  {
    Cars = _carService.ReadAll();
    return Partial("_CarPartial", Cars);
  }

}

