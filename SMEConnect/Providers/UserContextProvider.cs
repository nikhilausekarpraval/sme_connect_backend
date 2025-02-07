using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;
using Microsoft.AspNetCore.Identity;

namespace SMEConnect.Providers
{

    public class UserContextProvider
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DcimDevContext _dcimDevContext;

        public UserContextProvider(UserManager<ApplicationUser> userManager, DcimDevContext dcimDevContext)
        {
            _userManager = userManager;
            _dcimDevContext = dcimDevContext;
        }

        public class UserContextService : IUserContextService
        {
            public IUserContext UserContext { get; set; }
        }

        public string GetClaimValue(IHttpContextAccessor httpContextAccessor, string name)
        {
            return httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(s => s.Type.Contains(name))?.Value;
        }

        /// <summary>
        /// Generates the context.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>IUserContext.</returns>
        public async Task<IUserContext> GenerateContext(IServiceProvider serviceProvider)
        {
            var httpProvider = serviceProvider.GetService<IHttpContextAccessor>();

            UserContext u = new();
            if (httpProvider != null && httpProvider.HttpContext.User.Identity.IsAuthenticated)
            {
                u.Name = GetClaimValue(httpProvider, "name");
                u.Email = GetClaimValue(httpProvider, "emailaddress");

            }

            // Store the user context in the UserContextService
            var userContextService = serviceProvider.GetService<IUserContextService>();
            if (userContextService != null)
            {
                userContextService.UserContext = u;
            }


            return await GetLoggedInUser(u);
        }

        /// <summary>
        /// Gets the logged-in user, either from the database or the context.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>IUserContext.</returns>
        private async Task<IUserContext> GetLoggedInUser(IUserContext user)
        {
            if (user != null && !string.IsNullOrEmpty(user.Email))
            {
                // Check if the user exists in the Identity database
                var applicationUser = await _userManager.FindByEmailAsync(user.Email);

                if (applicationUser != null)
                {
                    // Update the user context with additional data if needed
                    user.Name = applicationUser.UserName;
                    user.Email = applicationUser.Email;
                    user.UserName = applicationUser.UserName;

                    return new UserContext
                    {
                        Email = user?.Email,
                        Name = user?.Name,
                        UserName = user?.UserName
                    };
                }
                else
                {
                    // Handle the case where the user does not exist in the database
                    // You may want to create a new user or return a default user context
                    return new UserContext
                    {
                        Email = user.Email,
                        Name = user.Name,
                        UserName = user.UserName
                    };
                }
            }

            return user;
        }
    }


}
