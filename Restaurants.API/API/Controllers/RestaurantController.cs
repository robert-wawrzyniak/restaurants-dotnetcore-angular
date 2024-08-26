using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Dtos;
using System;
using System.Threading.Tasks;

namespace Restaurants.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : BaseController
    {
        private readonly IRestaurantService restaurantService;
        private readonly IReviewService reviewService;

        public RestaurantController(IRestaurantService restaurantService, IReviewService reviewService)
        {
            this.restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var restaurants = await restaurantService.GetAllAsync();
            return Ok(restaurants);
        }

        [HttpGet("forcurrentuser")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetForOwner()
        {
            var restaurants = await restaurantService.GetOwnerRestaurantsAsync(CurrentUserId);
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var restaurant = await restaurantService.GetDetailsAsync(id);
            return Ok(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> CreateNewRestaurant([FromBody]RestaurantDto restaurantDto)
        {
            var id = await restaurantService.CreateRestaurantAsync(CurrentUserId, restaurantDto);
            return CreatedAtRoute(new { id }, id);
        }

        [HttpGet("{id}/reviews/{userId}")]
        public async Task<IActionResult> GetReview(Guid id, Guid userId)
        {
            var review = await reviewService.GetByIdAsync(id, userId);

            return Ok(review);
        }

        [HttpPost("{id}/reviews")]
        public async Task<IActionResult> ReviewRestaurant(Guid id, [FromBody]ReviewDto reviewDto)
        {
            var result = await reviewService.ReviewRestaurantAsync(CurrentUserId, id, reviewDto);
            if (!result.IsSuccess)
            {
                return ProcessResult(result);
            }

            return CreatedAtRoute($"{id}/reviews/{CurrentUserId}", "");
        }

        [HttpPost("{id}/reviews/{userId}/reply")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ReplyToReview(Guid id, Guid userId, [FromBody]ReplyDto reply)
        {
            var result = await reviewService.ReplyToReviewAsync(userId, id, CurrentUserId, reply.Reply);
            if (!result.IsSuccess)
            {
                return ProcessResult(result);
            }

            return Ok();
        }

        [HttpDelete("{id}/reviews/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReview(Guid id, Guid userId)
        {
            await reviewService.DeleteReviewAsync(userId, id);

            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Owner,Admin")]
        public async Task<IActionResult> UpdateRestaurant(Guid id, [FromBody]RestaurantDto restaurantDto)
        {
            var operationResult = await restaurantService.UpdateRestaurantAsync(id, CurrentUserId, restaurantDto);
            if (!operationResult.IsSuccess)
            {
                return ProcessResult(operationResult);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRestaurant(Guid id)
        {
            await restaurantService.DeleteAsync(id);

            return NoContent();
        }
    }
}