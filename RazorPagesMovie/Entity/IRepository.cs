
namespace RazorPages.Entity
{
  public interface IRepository<T> where T : class
  {
    Task CreateAsync(T entity);
    Task<List<T>> ReadAllAsync();
    Task<List<T>> ReadAsync(IQueryable<T> query);
  }
}