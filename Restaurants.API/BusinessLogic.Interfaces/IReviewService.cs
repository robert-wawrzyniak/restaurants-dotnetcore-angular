using Restaurants.Common;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewDto> GetByIdAsync(Guid restaurantId, Guid userId);
        Task<IEnumerable<ReviewDto>> GetPendingForOwnerAsync(Guid ownerId);
        Task<OperationResult> ReviewRestaurantAsync(Guid reviewerId, Guid restaurantId, ReviewDto reviewDto);
        Task<OperationResult> ReplyToReviewAsync(Guid reviewerId, Guid restaurantId, Guid currentUserId, string reply);
        Task DeleteReviewAsync(Guid reviewerId, Guid restaurantId);
    }
}
