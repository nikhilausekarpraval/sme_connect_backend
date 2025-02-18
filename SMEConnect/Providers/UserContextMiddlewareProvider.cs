using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using SMEConnect.Modals;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

namespace SMEConnect.Providers
{
    public class UserContextMiddlewareProvider
    {
        private readonly RequestDelegate _next;

        public UserContextMiddlewareProvider(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager,RoleManager<ApplicationRole> roleManager)
        {

            var authenticationResult = await context.AuthenticateAsync("AzureAD");  
            if (!authenticationResult.Succeeded)
            {
                authenticationResult = await context.AuthenticateAsync("CustomJwt");  // Fallback to CustomJwt scheme
            }

            if (authenticationResult.Succeeded)
            {
                context.User = authenticationResult.Principal;  // Set the authenticated user
            }
            else
            {
                context.User = new ClaimsPrincipal(new ClaimsIdentity());  // No user, create an empty principal
            }

            if (context.User.Identity.IsAuthenticated)
            {
                var email = context.User.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress") || c.Type.Contains("upn"))?.Value;

                if (!string.IsNullOrEmpty(email))
                {

                    var applicationUser = await userManager.FindByEmailAsync(email);
                    var userRoles = await userManager.GetRolesAsync(applicationUser);
                    var userClaims = await userManager.GetClaimsAsync(applicationUser);

                    var roleClaims = new List<Claim>();

                    foreach (var role in userRoles)
                    {
                        var roleEntity = await roleManager.FindByNameAsync(role);
                        if (roleEntity != null)
                        {
                            var claims = await roleManager.GetClaimsAsync(roleEntity);
                            if (claims != null)
                            {
                                roleClaims.AddRange(claims.Where(c => c != null));
                            }
                        }
                    }

                    if (applicationUser != null)
                    {
                        var userContext = new UserContext
                        {
                            Name = applicationUser?.UserName,
                            Email = applicationUser?.Email,
                            UserName = applicationUser?.UserName,
                            Roles = userRoles,
                            RoleClaims = roleClaims,
                            UserClaims = userClaims

                        };

                        context.Items["UserContext"] = userContext;
                    }
                }
            }

            await _next(context);
        }
    }
}
