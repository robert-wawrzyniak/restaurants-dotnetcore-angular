using Restaurants.Common;
using Restaurants.Dtos;
using System;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic.Interfaces
{
    public interface IAuthenticationService
    {
        Task<OperationResult<string>> GetTokenAsync(LoginDto loginDto);
        Task<OperationResult<Guid>> RegisterUserAsync(LoginDto registerDto);
    }
}
