using System;

namespace Restaurants.Dtos
{
    public class ReviewDto
    {
        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }
        public string Restaurant { get; set; }
        public string User { get; set; }
        public DateTimeOffset VisitDate { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string Reply { get; set; }
    }
}
