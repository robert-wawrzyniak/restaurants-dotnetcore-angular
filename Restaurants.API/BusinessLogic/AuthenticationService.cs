using Microsoft.IdentityModel.Tokens;
using Restaurants.BusinessLogic.Interfaces;
using Restaurants.Common;
using Restaurants.Common.Configuration;
using Restaurants.Common.Enum;
using Restaurants.DataAccess.Interfaces;
using Restaurants.DataAccess.Interfaces.Entities;
using Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.BusinessLogic
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserProvider userProvider;
        private readonly IRepository<User> userRepository;
        private readonly TokenConfiguration tokenConfiguration;

        public AuthenticationService(
            IUserProvider userProvider,
            IRepository<User> userRepository,
            TokenConfiguration tokenConfiguration)
        {
            this.userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.tokenConfiguration = tokenConfiguration ?? throw new ArgumentNullException(nameof(tokenConfiguration));
        }

        public async Task<OperationResult<string>> GetTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await userProvider.GetByNameAsync(loginDto.Name);
            if (user == null)
            {
                return ErrorResult;
            }
            var passwordHash = CalculatePasswordHash(loginDto.Password);
            if (passwordHash != user.Password)
            {
                return ErrorResult;
            }

            var token = GenerateToken(user);
            return new OperationResult<string>(token);
        }

        public async Task<OperationResult<Guid>> RegisterUserAsync(LoginDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto));
            }

            var isUserExisting = await userProvider.IsUserExistingAsync(registerDto.Name);
            if (isUserExisting)
            {
                return new OperationResult<Guid>("User already exists in the system", FailureReason.BadRequest);
            }

            var hashedPassword = CalculatePasswordHash(registerDto.Password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = registerDto.Name,
                Password = hashedPassword
            };

            await userRepository.AddAsync(user);

            return new OperationResult<Guid>(user.Id);
        }

        private string CalculatePasswordHash(string password)
        {
            var myhmacsha1 = new HMACSHA512(Key);
            byte[] byteArray = Encoding.ASCII.GetBytes(password);
            var stream = new MemoryStream(byteArray);
            return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        }

        private string GenerateToken(User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            }.Concat((user.Roles ?? new List<UserRole>()).Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private byte[] Key
            => Encoding.ASCII.GetBytes(tokenConfiguration.Secret);

        private OperationResult<string> ErrorResult
            => new OperationResult<string>("Wrong login or password", FailureReason.BadRequest);
    }
}
