
namespace RazorPages.Entity
{
  public interface IRepository<T> where T : class
  {
    Task CreateAsync(T entity);
    Task<List<T>> ReadAllAsync();
    Task<List<T>> ReadAsync(IQueryable<T> query);

    Task<(List<T>, int)> ReadAllFilterAsync(int skip, int take);
    Task<(List<T>, int)> ReadPageAsync(IQueryable<T> query, int skip, int take);
  }
}