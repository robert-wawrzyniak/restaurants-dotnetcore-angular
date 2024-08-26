using Microsoft.EntityFrameworkCore;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.DataAccess.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.DataAccess.Entity
{
    public class RestaurantProvider : IRestaurantProvider
    {
        private readonly RestaurantsDbContext dbContext;

        public RestaurantProvider(RestaurantsDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Restaurant> GetByIdAsync(Guid id)
        {
            return await dbContext.Restaurants.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<RestaurantDetails> GetRestaurantDetailsAsync(Guid id)
        {
            var restaurantDetails = await dbContext.Restaurants
                .Where(e => e.Id == id && e.Reviews.Any())
                .Select(e => new RestaurantDetails
                {
                    Id = e.Id,
                    Name = e.Name,
                    City = e.City,
                    Address = e.Address,
                    Owner = e.Owner,
                    AverageRating = e.Reviews.Select(r => r.Rate).Average(),
                    HighestRatedReview = e.Reviews.OrderByDescending(r => r.Rate).FirstOrDefault(),
                    LowestRatedReview = e.Reviews.OrderBy(r => r.Rate).FirstOrDefault(),
                    LastReviews = e.Reviews.OrderByDescending(r => r.VisitDate).Take(5).ToList()
                })
                .Union(
                dbContext.Restaurants
                .Where(e => e.Id == id && !e.Reviews.Any())
                .Select(e => new RestaurantDetails
                {
                    Id = e.Id,
                    Name = e.Name,
                    City = e.City,
                    Address = e.Address,
                    Owner = e.Owner
                }))
                .FirstOrDefaultAsync();

            return restaurantDetails;
        }

        public async Task<IEnumerable<Restaurant>> GetAllOrderedByRateAverageAsync()
        {
            var restaurants = await dbContext.Restaurants
                .Where(e => e.Reviews.Any())
                .OrderByDescending(e => e.Reviews.Select(r => r.Rate).Average())
                .Concat(
                    dbContext.Restaurants
                    .Where(e => !e.Reviews.Any()))
                .ToListAsync();

            return restaurants;
        }
        
        public async Task<IEnumerable<Restaurant>> GetOwnerRestaurantsAsync(Guid ownerId)
        {
            var restaurants = await dbContext.Restaurants
                .Where(e => e.OwnerId == ownerId)
                .ToListAsync();

            return restaurants;
        }
    }
}
