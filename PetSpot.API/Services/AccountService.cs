using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PetSpot.API.Configuration;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Exceptions;
using PetSpot.DATA.Models;
using PetSpot.LOGGING;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetSpot.API.Services
{
    /// <inheritdoc/>
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly JwtSettings jwtSettings;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;

        public AccountService(UserManager<User> userManager,
            RoleManager<UserRole> roleManager,
            JwtSettings jwtSettings,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.jwtSettings = jwtSettings;
            this.mapper = mapper;
            this.logger = logger;
        }


        /// <summary>
        /// Method used for user authorization. UserManager checks
        /// if user with the username exists and checks password.
        /// If authorization is successful, creates a token and returns
        /// it.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Authorization response DTO</returns>
        public async Task<AuthorizeResponseDto> Authorize(AuthorizeBm model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var token = await CreateTokenAsync(user);
                var response = mapper.Map<AuthorizeResponseDto>(token);
                response.UserName = user.UserName;

                if (response != null)
                {
                    logger.LogInfo($"AUTHORIZATION SUCCESSFUL : User {model.UserName} successfully authorized.");
                    return response;
                }
            }

            throw new BadRequestException("Invalid username or password.");
        }

        /// <summary>
        /// Creates a new user and adds the role of registered user.
        /// Throws exceptions if UserName already exists and if
        /// email already exists
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Register(RegisterUserBm model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
            {
                logger.LogInfo($"REGISTRATION FAILED : user with username {model.UserName} already exists.");
                throw new BadRequestException($"A user with username {model.UserName} already exists.");
            }

            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
            {
                logger.LogInfo($"REGISTRATION FAILED : user with email {model.Email} already exists.");
                throw new BadRequestException($"A user with email {model.Email} already exists.");
            }

            var user = mapper.Map<User>(model);
            user.SecurityStamp = Guid.NewGuid().ToString();

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Exception("Internal Server Error");
            }

            if (await roleManager.RoleExistsAsync("Registered user"))
            {
                await userManager.AddToRoleAsync(user, "Registered user");
            }

            logger.LogInfo($"REGISTRATION SUCCESSFUL : user with email {model.Email} already exists.");
        }

        // Creates a JWT token asynchronously
        private async Task<TokenResultDto> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var userRoles = await userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(7);
            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            logger.LogInfo($"JWT Bearer Token successfully created.");
            return new TokenResultDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            };
        }
    }
}
