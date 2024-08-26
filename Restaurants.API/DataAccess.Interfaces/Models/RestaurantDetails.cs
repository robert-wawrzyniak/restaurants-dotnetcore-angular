using Restaurants.DataAccess.Interfaces.Entities;
using System;
using System.Collections.Generic;

namespace Restaurants.DataAccess.Interfaces.Models
{
    public class RestaurantDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public User Owner { get; set; }
        public double AverageRating { get; set; }
        public Review HighestRatedReview { get; set; }
        public Review LowestRatedReview { get; set; }
        public IEnumerable<Review> LastReviews { get; set; }
    }
}
