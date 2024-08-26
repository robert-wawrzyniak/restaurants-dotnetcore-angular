using Restaurants.DataAccess.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Interfaces
{
    public interface IReviewProvider
    {
        Task<Review> GetByIdsAsync(Guid userId, Guid restaurantId);
        Task<IEnumerable<Review>> GetPendingReviewsForOwnerAsync(Guid ownerId);
        Task<bool> HasUserReviewedRestaurantAsync(Guid userId, Guid restaurantId);
    }
}
