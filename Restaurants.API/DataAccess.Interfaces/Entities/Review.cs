using System;

namespace Restaurants.DataAccess.Interfaces.Entities
{
    public class Review
    {
        public Guid RestaurantId { get; set; }
        public Guid UserId { get; set; }
        public Restaurant Restaurant { get; set; }
        public User User { get; set; }
        public DateTimeOffset VisitDate { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public string Reply { get; set; }
    }
}
