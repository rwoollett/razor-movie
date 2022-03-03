#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RazorPages.Models;

    public class MovieContext : IdentityDbContext
    {
        public MovieContext (DbContextOptions<MovieContext> options)
            : base(options)
        {
        }

        public DbSet<RazorPages.Models.Movie> Movie { get; set; }
        public DbSet<RazorPages.Models.Car> Car { get; set; }
    }
