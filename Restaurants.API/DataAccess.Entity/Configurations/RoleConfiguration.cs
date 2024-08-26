using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.DataAccess.Interfaces.Entities;
using System;

namespace Restaurants.DataAccess.Entity.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();

            builder.HasData(
                new Role { Id = new Guid("f47d1d50-4c33-49ec-9f4d-eb94ec019cd2"), Name = "Owner" },
                new Role { Id = new Guid("f3ea23f3-0d9e-4f90-834c-3eb419befd1a"), Name = "Admin" });
        }
    }
}
