#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.Models;
using RazorPages.Entity;

namespace RazorPages.Pages.Movies
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Movie> repository;

        public CreateModel(IRepository<Movie> repository)
        {
            this.repository = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await repository.CreateAsync(Movie);
            return RedirectToPage("./Index");
        }
    }
}
