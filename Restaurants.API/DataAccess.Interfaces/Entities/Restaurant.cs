using System;
using System.Collections.Generic;

namespace Restaurants.DataAccess.Interfaces.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
