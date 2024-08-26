using Microsoft.EntityFrameworkCore;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Entity
{
    public class UserProvider : IUserProvider
    {
        private readonly RestaurantsDbContext dbContext;
        
        public UserProvider(RestaurantsDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await NonDeletedUserQuery
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await UserPermissionsQuery
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var user = await UserPermissionsQuery
                .FirstOrDefaultAsync(u => u.Name == name);

            return user;
        }

        public async Task<bool> IsUserExistingAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return await NonDeletedUserQuery
                .AnyAsync(u => u.Name == name);
        }

        private IQueryable<User> NonDeletedUserQuery
            => dbContext.Users
            .Where(e => !e.IsDeleted);

        private IQueryable<User> UserPermissionsQuery
            => dbContext.Users
            .Include(e => e.Roles)
            .ThenInclude(r => r.Role)
            .Where(e => !e.IsDeleted);
    }
}
