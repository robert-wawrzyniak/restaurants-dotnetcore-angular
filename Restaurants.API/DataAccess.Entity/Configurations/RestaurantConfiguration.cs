using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.DataAccess.Interfaces.Entities;

namespace Restaurants.DataAccess.Entity.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(e => e.Id);
            builder
                .HasOne(e => e.Owner)
                .WithMany()
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.City).IsRequired();
            builder.Property(e => e.Address).IsRequired();
        }
    }
}
