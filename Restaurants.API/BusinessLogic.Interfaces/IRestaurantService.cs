using Restaurants.Common;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic.Interfaces
{
    public interface IRestaurantService
    {
        Task<IEnumerable<RestaurantDto>> GetAllAsync();
        Task<IEnumerable<RestaurantDto>> GetOwnerRestaurantsAsync(Guid ownerId);
        Task<RestaurantDetailsDto> GetDetailsAsync(Guid id);
        Task<Guid> CreateRestaurantAsync(Guid ownerId, RestaurantDto restaurantDto);
        Task<OperationResult> UpdateRestaurantAsync(Guid id, Guid currentUserId, RestaurantDto restaurantDto);
        Task DeleteAsync(Guid id);
    }
}
