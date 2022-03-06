#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;
using RazorPages.Entity;

namespace RazorPages.Pages.Movies
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<Movie> repository;
        private readonly IRepository<Tag> tag_repository;

        public DeleteModel(IRepository<Movie> repository,
        IRepository<Tag> tag_repository)
        {
          this.repository = repository;
          this.tag_repository = tag_repository;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
          try 
          {
            Movie = await repository.ReadAsync(id);
          }
          catch (ArgumentException)
          {
              return NotFound();
          }
          return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
          try 
          {
            Movie = await repository.ReadAsync(id);
          }
          catch (ArgumentException)
          {
              return NotFound();
          }
          foreach (var t in Movie.Tags)
          { 
            await tag_repository.DeleteAsync(t);
          }
          await repository.DeleteAsync(Movie);
          return RedirectToPage("./Index", new { PageNumber = 1 });
        }
    }
}
