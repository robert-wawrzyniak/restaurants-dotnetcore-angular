using System;

namespace Restaurants.Dtos
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
    }
}
