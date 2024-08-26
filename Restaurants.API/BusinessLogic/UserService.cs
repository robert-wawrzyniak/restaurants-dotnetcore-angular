using AutoMapper;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Common;
using Restaurants.Common.Enum;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserProvider userProvider;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRolesRepository;
        private readonly IMapper mapper;

        public UserService(
            IUserProvider userProvider,
            IRepository<User> userRepository,
            IRepository<UserRole> userRolesRepository,
            IMapper mapper)
        {
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.userRolesRepository = userRolesRepository ?? throw new ArgumentNullException(nameof(userRolesRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await userProvider.GetAllAsync();

            return mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserPermissionsDto> GetByIdAsync(Guid id)
        {
            var users = await userProvider.GetByIdAsync(id);

            return mapper.Map<UserPermissionsDto>(users);
        }

        public async Task<OperationResult> UpdateAsync(Guid id, UserPermissionsDto userPermissionsDto)
        {
            if (userPermissionsDto == null)
            {
                throw new ArgumentNullException(nameof(userPermissionsDto));
            }

            var user = await userProvider.GetByIdAsync(id);
            if (user == null)
            {
                return new OperationResult("User does not exist", FailureReason.BadRequest);
            }
            user.Name = userPermissionsDto.Name;

            await userRepository.UpdateAsync(user);

            if (userPermissionsDto.IsAdmin)
            {
                await AssureThatRoleIsAssignedToUser(userPermissionsDto, user, "Admin", new Guid("f3ea23f3-0d9e-4f90-834c-3eb419befd1a"));
            }
            else
            {
                await AssureThatRoleIsRevokedFromUser(userPermissionsDto, user, "Admin", new Guid("f3ea23f3-0d9e-4f90-834c-3eb419befd1a"));
            }

            if (userPermissionsDto.IsOwner)
            {
                await AssureThatRoleIsAssignedToUser(userPermissionsDto, user, "Owner", new Guid("f47d1d50-4c33-49ec-9f4d-eb94ec019cd2"));
            }
            else
            {
                await AssureThatRoleIsRevokedFromUser(userPermissionsDto, user, "Owner", new Guid("f47d1d50-4c33-49ec-9f4d-eb94ec019cd2"));
            }

            return OperationResult.Success;
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            var user = await userProvider.GetByIdAsync(id);
            if (user == null)
            {
                return new OperationResult("User does not exist", FailureReason.BadRequest);
            }

            user.IsDeleted = true;
            await userRepository.UpdateAsync(user);

            return OperationResult.Success;
        }

        private async Task AssureThatRoleIsAssignedToUser(
            UserPermissionsDto userPermissionsDto, User user, string roleName, Guid roleId)
        {
            if (!user.Roles.Any(r => r.Role.Name == roleName))
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                };
                await userRolesRepository.AddAsync(userRole);
            }
        }

        private async Task AssureThatRoleIsRevokedFromUser(
            UserPermissionsDto userPermissionsDto, User user, string roleName, Guid roleId)
        {
            if (user.Roles.Any(r => r.Role.Name == roleName))
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = roleId
                };
                await userRolesRepository.DeleteAsync(userRole);
            }
        }
    }
}
