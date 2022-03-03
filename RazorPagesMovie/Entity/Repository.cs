using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RazorPages.Entity
{
  public class Repository<T> : IRepository<T> where T : class{
    private MovieContext context;

    public Repository(MovieContext context)
    {
      this.context = context;
    }

    public async Task CreateAsync(T entity)
    {
      if (entity == null)
        throw new ArgumentException(nameof(entity));
    
      context.Add(entity);
      await context.SaveChangesAsync();
    }

    public async Task<List<T>> ReadAllAsync() 
    {
      return await context.Set<T>().ToListAsync();
    }

    public async Task<List<T>> ReadAsync(IQueryable<T> query) 
    {
      return await query.ToListAsync();
    }
  }
}