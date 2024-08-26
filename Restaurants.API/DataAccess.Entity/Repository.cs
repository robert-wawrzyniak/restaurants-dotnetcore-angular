using Microsoft.EntityFrameworkCore;
using Restaurants.DataAccess.Interfaces;
using System;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Entity
{
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly RestaurantsDbContext dbContext;

        public Repository(RestaurantsDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task AddAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Set.Add(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Set.Update(item);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            Set.Remove(item);
            await dbContext.SaveChangesAsync();
        }

        private DbSet<T> Set
            => dbContext.Set<T>();
    }
}
