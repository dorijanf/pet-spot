using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PetSpot.API.Configuration
{
    /// <summary>
    /// Resolver service that makes it possible to get information about the currently logged in user
    /// </summary>
    public class UserResolverService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal GetUser()
        {
            return httpContextAccessor.HttpContext.User;
        }
    }
}
