using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.DataAccess.Interfaces.Entities;

namespace Restaurants.DataAccess.Entity.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Password).IsRequired();
            builder.HasIndex(e => e.Name).IsUnique();
        }
    }
}
