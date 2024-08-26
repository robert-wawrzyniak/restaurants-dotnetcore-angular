using Microsoft.AspNetCore.Mvc;
using Restaurants.Common;
using Restaurants.Common.Enum;
using System;

namespace Restaurants.API.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected Guid CurrentUserId
            => new Guid(HttpContext.User.Identity.Name);

        protected IActionResult ProcessResult(OperationResult operationResult)
        {
            if (operationResult.IsSuccess)
            {
                return Ok();
            }

            switch(operationResult.FailureReason.Value)
            {
                case FailureReason.BadRequest:
                    return BadRequest();
                case FailureReason.Unauthorized:
                    return Unauthorized();
            }

            return StatusCode(500);
        }
    }
}
