using System.Threading.Tasks;

namespace Restaurants.DataAccess.Interfaces
{
    public interface IRepository<T>
        where T : class
    {
        Task AddAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(T item);
    }
}
