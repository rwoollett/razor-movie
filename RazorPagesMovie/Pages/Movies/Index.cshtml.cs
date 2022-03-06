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
using System.Linq.Expressions; 

namespace RazorPages.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly IRepository<Movie> repository;
        private readonly MovieContext context;

        public IndexModel(MovieContext context,
                          IRepository<Movie> repository)
        {
            this.repository = repository;
            this.context = context;
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

            var filterList = new List<Expression<Func<Movie, bool>>>();
          
            if (!string.IsNullOrEmpty(SearchString))
            {
              filterList.Add(s => s.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre)) 
            {
              filterList.Add(x => x.Genre == MovieGenre);
            }

            var query = repository.ReadAll(filterList);
            
            var resultTuple = await repository.ReadPageAsync(query, skip, pageSize);
            pagingInfo.TotalItems = resultTuple.Item2;
            MovieList.movie = resultTuple.Item1;
            MovieList.pagingInfo = pagingInfo;

            TitleMovie = MovieList.movie.Count() > 0 ? MovieList.movie.First<Movie>() : new Movie();
            
            IQueryable<string> genreQuery = from m in context.Movie
                                    orderby m.Genre
                                    select m.Genre;
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync());

        }

        // TODO: temporary delete all for seeding dev db
        public async Task<IActionResult> OnPostDeleteAllAsync()
        {
          var movies = from m in context.Movie
                         select m;
          context.Movie.RemoveRange(movies.ToList());
          await context.SaveChangesAsync();
          return RedirectToPage("./Index");
        }

    }
}
