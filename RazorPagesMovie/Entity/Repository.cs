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

    public async Task<(List<T>, int)> ReadPageAsync(IQueryable<T> query, int skip, int take) 
    {
      var relevant = await query.Skip(skip).Take(take).ToListAsync();
      var total = query.Count();

      (List<T>, int) result = (relevant, total);

      return result;    
    }


    public IQueryable<T> ReadAll(List<Expression<Func<T, bool>>>? filterList)
    {
      if (filterList == null || filterList.Count == 0)
        return context.Set<T>();

      IQueryable<T> entities = context.Set<T>();
      foreach (var filter in filterList) {
          entities = entities.Where(filter); 
      }
      return entities;
    }

    public async Task UpdateAsync(T entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof(entity));
 
      context.Update(entity);
      await context.SaveChangesAsync();
    }

    public async Task<T> ReadAsync(int id)
    {
      var entity = await context.Set<T>().FindAsync(id);
      if (entity == null) 
      {
        throw new ArgumentException(nameof(entity));
      }

      // TODO: Load the tags related to a given movie.
      context.Entry(entity).Collection("Tags").Load();
      return entity;
    }

    public async Task DeleteAsync(T entity)
    {
      if (entity == null)
        throw new ArgumentNullException(nameof(entity));
      context.Set<T>().Remove(entity);
      await context.SaveChangesAsync();
    }

  }
}