using Restaurants.DataAccess.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Interfaces
{
    public interface IUserProvider
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> IsUserExistingAsync(string name);
        Task<User> GetByNameAsync(string name);
        Task<User> GetByIdAsync(Guid id);
    }
}
