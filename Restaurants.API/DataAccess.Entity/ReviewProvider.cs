using Microsoft.EntityFrameworkCore;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Entity
{
    public class ReviewProvider : IReviewProvider
    {
        private readonly RestaurantsDbContext dbContext;

        public ReviewProvider(RestaurantsDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Review> GetByIdsAsync(Guid userId, Guid restaurantId)
        {
            return await dbContext.Reviews.FirstOrDefaultAsync(e => e.UserId == userId && e.RestaurantId == restaurantId);
        }

        public async Task<IEnumerable<Review>> GetPendingReviewsForOwnerAsync(Guid ownerId)
        {
            var reviews = await dbContext.Reviews
                .Where(e =>
                    e.Reply == null
                    && e.Restaurant.OwnerId == ownerId)
                .ToListAsync();

            return reviews;
        }

        public async Task<bool> HasUserReviewedRestaurantAsync(Guid userId, Guid restaurantId)
        {
            return await dbContext.Reviews.AnyAsync(e => e.UserId == userId && e.RestaurantId == restaurantId);
        }
    }
}
