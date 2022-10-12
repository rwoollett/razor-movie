using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;
using RazorPages.Entity;


namespace RazorPages.Pages.Movies
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Movie> repository;

        public EditModel(IRepository<Movie> repository)
        {
            this.repository = repository;
        }

        [BindProperty]
        public Movie Movie { get; set; } = new Movie();

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await repository.UpdateAsync(Movie);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! await MovieExists(Movie.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { PageNumber = 1 });
        }

        private async Task<bool> MovieExists(int id)
        {
            try 
            {
              Movie = await repository.ReadAsync(id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
