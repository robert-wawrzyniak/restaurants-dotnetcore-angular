using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Dtos;
using System;
using System.Threading.Tasks;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IAuthenticationService authenticationService;
        private readonly IReviewService reviewService;
        private readonly IUserService userService;

        public UserController(
            IAuthenticationService authenticationService,
            IReviewService reviewService,
            IUserService userService)
        {
            this.authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            this.reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var users = await userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await userService.GetByIdAsync(id);
            return Ok(user);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUserPermissions()
        {
            var user = await userService.GetByIdAsync(CurrentUserId);
            return Ok(user);
        }

        [HttpGet("me/restaurants/reviews/pending")]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> GetPendingReviewsForCurrentUserRestaurant()
        {
            var pendingReviews = await reviewService.GetPendingForOwnerAsync(CurrentUserId);
            return Ok(pendingReviews);
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody]LoginDto loginDto)
        {
            var tokenResult = await authenticationService.GetTokenAsync(loginDto);
            return Ok(tokenResult.Result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]LoginDto registerDto)
        {
            var idResult = await authenticationService.RegisterUserAsync(registerDto);

            if (!idResult.IsSuccess)
            {
                return ProcessResult(idResult);
            }

            return CreatedAtRoute(new { id = idResult.Result }, idResult.Result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody]UserPermissionsDto userDto)
        {
            var result = await userService.UpdateAsync(id, userDto);

            if (!result.IsSuccess)
            {
                return ProcessResult(result);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await userService.DeleteAsync(id);

            if (!result.IsSuccess)
            {
                return ProcessResult(result);
            }

            return NoContent();
        }
    }
}