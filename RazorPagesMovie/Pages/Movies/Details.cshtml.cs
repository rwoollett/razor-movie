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
    public class DetailsModel : PageModel
    {
        private readonly IRepository<Movie> repository;

        public DetailsModel(IRepository<Movie> repository)
        {
          this.repository = repository;
        }

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
    }
}
