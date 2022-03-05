#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;
using RazorPages.Entity;
using RazorPages.Paging;

namespace RazorPages.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Movie> repository;
        private readonly MovieContext _context;

        public IndexModel(MovieContext context,
                          IRepository<Movie> repository)
        {
            _context = context;
            this.repository = repository;
        }

        public MovieList MovieList { get;set; }
        public Movie TitleMovie { get; set; }

        [BindProperty(SupportsGet = true)] 
        public string SearchString { get; set; }
        public SelectList Genres { get; set; }

        [BindProperty(SupportsGet = true)] 
        public string MovieGenre { get; set; }

        [BindProperty(SupportsGet = true)] 
        public int PageNumber { get; set; }

        public async Task OnGetAsync()
        {
            MovieList = new MovieList();

            int pageSize = 3;
            PagingInfo pagingInfo = new PagingInfo();
            pagingInfo.CurrentPage = PageNumber == 0 ? 1 : PageNumber;
            pagingInfo.ItemsPerPage = pageSize;
            var skip = pageSize * (Convert.ToInt32(PageNumber) - 1);

            IQueryable<string> genreQuery = from m in _context.Movie
                                    orderby m.Genre
                                    select m.Genre;

            var movies = from m in _context.Movie
                         select m;

            if (!string.IsNullOrEmpty(SearchString))
            {
              movies = movies.Where(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre)) 
            {
              movies = movies.Where(x => x.Genre == MovieGenre);
            }

            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());
            
            var resultTuple = await repository.ReadPageAsync(movies, skip, pageSize);
            pagingInfo.TotalItems = resultTuple.Item2;
            MovieList.movie = resultTuple.Item1;
            MovieList.pagingInfo = pagingInfo;

            TitleMovie = MovieList.movie.Count() > 0 ? MovieList.movie.First<Movie>() : new Movie();

        }

        public async Task<IActionResult> OnPostAsync()
        {
          var movies = from m in _context.Movie
                         select m;
          _context.Movie.RemoveRange(movies.ToList());
          await _context.SaveChangesAsync();
          return RedirectToPage("./Index");
        }

    }
}
