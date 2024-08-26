using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Interfaces
{
    public interface IRestaurantProvider
    {
        Task<Restaurant> GetByIdAsync(Guid id);
        Task<RestaurantDetails> GetRestaurantDetailsAsync(Guid id);
        Task<IEnumerable<Restaurant>> GetAllOrderedByRateAverageAsync();
        Task<IEnumerable<Restaurant>> GetOwnerRestaurantsAsync(Guid ownerId);
    }
}
