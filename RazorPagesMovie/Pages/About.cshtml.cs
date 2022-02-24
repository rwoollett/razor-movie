using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesMovie.Pages;

public class AboutModel : PageModel
{
    public string? Msg { get; set; }

    // public void OnGet()
    // {
    //   Msg = "The about page.";
    // }

    public IActionResult OnGet()
    {
        Msg = "The about page.";
        return Page();
    }
}

