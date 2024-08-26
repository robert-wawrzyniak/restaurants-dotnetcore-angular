using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.DataAccess.Interfaces.Entities;

namespace Restaurants.DataAccess.Entity.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder
                .HasKey(e => new { e.UserId, e.RestaurantId });
            builder
                .HasOne(e => e.Restaurant)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict);
            builder
                .HasOne(e => e.User)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.Comment).IsRequired(false);
            builder.Property(e => e.Reply).IsRequired(false);
        }
    }
}
