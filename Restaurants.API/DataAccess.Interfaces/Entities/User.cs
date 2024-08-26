using System;
using System.Collections.Generic;

namespace Restaurants.DataAccess.Interfaces.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
