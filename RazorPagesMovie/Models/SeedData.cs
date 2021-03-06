using Microsoft.EntityFrameworkCore;

namespace RazorPages.Models
{
    public static class SeedData
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MovieContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MovieContext>>()))
            {
                if (context == null || context.Movie == null)
                {
                    throw new ArgumentNullException("Null RazorPagesMovieContext");
                }

                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M,
                        Rating = "R"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M,
                        Rating = "G"
                    },

                    new Movie
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M,
                        Rating = "NA"
                    }
                );


                var listMovies = new List<Movie>();
                for (int i = 0; i < 50; i++) {
                  listMovies.Add(new Movie
                    {
                        Title = "My new movie " + i.ToString(),
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 2.99M,
                        Rating = "NA"
                    });
                }
                context.Movie.AddRange(listMovies);
                await context.SaveChangesAsync();
                var movie = context.Movie.First(m => m.Title.Equals("My new movie 1"));
                for (int i = 0; i < 5; i++) {
                  var tag = new Tag
                    {
                      Name = "My tag " + i.ToString(),
                      Movie = movie
                    };
                  context.Tag.Add(tag);
                  movie.Tags.Add(tag);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}