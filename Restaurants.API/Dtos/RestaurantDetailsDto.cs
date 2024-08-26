using System.Collections.Generic;

namespace Restaurants.Dtos
{
    public class RestaurantDetailsDto : RestaurantDto
    {
        public UserDto Owner { get; set; }
        public double AverageRating { get; set; }
        public ReviewDto HighestRatedReview { get; set; }
        public ReviewDto LowestRatedReview { get; set; }
        public IEnumerable<ReviewDto> LastReviews { get; set; }
    }
}
