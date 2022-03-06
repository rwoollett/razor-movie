
namespace RazorPages.Entity
{
  public interface IRepository<T> where T : class
  {
    Task CreateAsync(T entity);
    Task<(List<T>, int)> ReadPageAsync(IQueryable<T> query, int skip, int take);

    Task UpdateAsync(T entity);

    Task<T> ReadAsync(int id);
    Task DeleteAsync(T entity);
  }
}