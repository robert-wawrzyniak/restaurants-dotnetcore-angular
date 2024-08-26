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
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantProvider restaurantProvider;
        private readonly IRepository<Restaurant> restaurantRepository;
        private readonly IMapper mapper;

        public RestaurantService(
            IRestaurantProvider restaurantProvider,
            IRepository<Restaurant> restaurantRepository,
            IMapper mapper)
        {
            this.restaurantProvider = restaurantProvider ?? throw new ArgumentNullException(nameof(restaurantProvider));
            this.restaurantRepository = restaurantRepository ?? throw new ArgumentNullException(nameof(restaurantRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllAsync()
        {
            var restaurants = await restaurantProvider.GetAllOrderedByRateAverageAsync();

            return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }

        public async Task<IEnumerable<RestaurantDto>> GetOwnerRestaurantsAsync(Guid ownerId)
        {
            var restaurants = await restaurantProvider.GetOwnerRestaurantsAsync(ownerId);

            return mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        }

        public async Task<Guid> CreateRestaurantAsync(Guid ownerId, RestaurantDto restaurantDto)
        {
            if (restaurantDto == null)
            {
                throw new ArgumentNullException(nameof(restaurantDto));
            }

            var restaurant = mapper.Map<Restaurant>(restaurantDto);
            restaurant.Id = Guid.NewGuid();
            restaurant.OwnerId = ownerId;

            await restaurantRepository.AddAsync(restaurant);

            return restaurant.Id;
        }

        public async Task<RestaurantDetailsDto> GetDetailsAsync(Guid id)
        {
            var restaurant = await restaurantProvider.GetRestaurantDetailsAsync(id);
            return mapper.Map<RestaurantDetailsDto>(restaurant);
        }

        public async Task<OperationResult> UpdateRestaurantAsync(Guid id, Guid currentUserId, RestaurantDto restaurantDto)
        {
            if (restaurantDto == null)
            {
                throw new ArgumentNullException(nameof(restaurantDto));
            }

            var restaurant = await restaurantProvider.GetByIdAsync(id);

            if(restaurant.OwnerId != currentUserId)
            {
                return new OperationResult("User not authorized to alter this restaurant", FailureReason.Unauthorized);
            }

            restaurant.Name = restaurantDto.Name;
            restaurant.City = restaurantDto.City;
            restaurant.Address = restaurantDto.Address;

            await restaurantRepository.UpdateAsync(restaurant);

            return OperationResult.Success;
        }

        public async Task DeleteAsync(Guid id)
        {
            var restaurant = new Restaurant
            {
                Id = id
            };
            await restaurantRepository.DeleteAsync(restaurant);
        }
    }
}
