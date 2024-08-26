using AutoMapper;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Common;
using Restaurants.Common.Enum;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic
{
    public class ReviewService : IReviewService
    {
        private readonly IRestaurantProvider restaurantProvider;
        private readonly IReviewProvider reviewProvider;
        private readonly IRepository<Review> reviewRepository;
        private readonly IMapper mapper;

        public ReviewService(
            IRestaurantProvider restaurantProvider,
            IReviewProvider reviewProvider,
            IRepository<Review> reviewRepository,
            IMapper mapper)
        {
            this.restaurantProvider = restaurantProvider ?? throw new ArgumentNullException(nameof(restaurantProvider));
            this.reviewProvider = reviewProvider ?? throw new ArgumentNullException(nameof(reviewProvider));
            this.reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReviewDto> GetByIdAsync(Guid restaurantId, Guid userId)
        {
            var review = await reviewProvider.GetByIdsAsync(userId, restaurantId);

            return mapper.Map<ReviewDto>(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetPendingForOwnerAsync(Guid ownerId)
        {
            var pendingReviews = await reviewProvider.GetPendingReviewsForOwnerAsync(ownerId);

            return mapper.Map<IEnumerable<ReviewDto>>(pendingReviews);
        }

        public async Task<OperationResult> ReplyToReviewAsync(Guid reviewerId, Guid restaurantId, Guid currentUserId, string reply)
        {
            if (string.IsNullOrEmpty(reply))
            {
                return new OperationResult("No reply", FailureReason.BadRequest);
            }

            var review = await reviewProvider.GetByIdsAsync(reviewerId, restaurantId);
            if (review == null)
            {
                return new OperationResult("User has not reviewed this restaurant", FailureReason.BadRequest);
            }

            var restaurant = await restaurantProvider.GetByIdAsync(restaurantId);
            if (restaurant == null || restaurant.OwnerId != currentUserId)
            {
                return new OperationResult("Wrong request", FailureReason.BadRequest);
            }

            review.Reply = reply;
            await reviewRepository.UpdateAsync(review);

            return OperationResult.Success;
        }

        public async Task<OperationResult> ReviewRestaurantAsync(Guid reviewerId, Guid restaurantId, ReviewDto reviewDto)
        {
            var restaurant = await restaurantProvider.GetByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return new OperationResult("Restaurant does not exist", FailureReason.BadRequest);
            }
            if (restaurant.OwnerId == reviewerId)
            {
                return new OperationResult("Owner cannot review its own restaurant", FailureReason.BadRequest);
            }
            if (await reviewProvider.HasUserReviewedRestaurantAsync(reviewerId, restaurantId))
            {
                return new OperationResult("Restaurant already reviewed", FailureReason.BadRequest);
            }
            if (reviewDto.Rate < 1 || reviewDto.Rate > 5)
            {
                return new OperationResult("Wrong rate", FailureReason.BadRequest);
            }

            var review = new Review
            {
                UserId = reviewerId,
                RestaurantId = restaurantId,
                Rate = reviewDto.Rate,
                Comment = reviewDto.Comment,
                VisitDate = DateTimeOffset.Now
            };
            await reviewRepository.AddAsync(review);

            return OperationResult.Success;
        }

        public async Task DeleteReviewAsync(Guid reviewerId, Guid restaurantId)
        {
            var review = await reviewProvider.GetByIdsAsync(reviewerId, restaurantId);
            if (review == null)
            {
                return;
            }

            await reviewRepository.DeleteAsync(review);
        }
    }
}
