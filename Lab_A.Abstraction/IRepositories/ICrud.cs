namespace Lab_A.Abstraction.IRepositories;

public interface ICrud<T>
{
    Task<T> CreateAsync(T entity);
    Task<T?> ReadAsync(int id);
    Task<IEnumerable<T>> ReadAllAsync();
    Task<T?> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
    Task<bool> DeleteAllAsync();
}