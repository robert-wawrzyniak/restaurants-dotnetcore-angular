using Restaurants.Common;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserPermissionsDto> GetByIdAsync(Guid id);
        Task<OperationResult> UpdateAsync(Guid id, UserPermissionsDto userPermissionsDto);
        Task<OperationResult> DeleteAsync(Guid id);
    }
}
