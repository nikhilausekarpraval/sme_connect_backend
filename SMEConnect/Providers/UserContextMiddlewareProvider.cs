using Microsoft.AspNetCore.Identity;
using SMEConnect.Modals;
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
            if (context.User.Identity.IsAuthenticated)
            {
                var email = context.User.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;

                if (!string.IsNullOrEmpty(email))
                {

                    var applicationUser = await userManager.FindByEmailAsync(email);
                    var userRoles = await userManager.GetRolesAsync(applicationUser);
                    var userClaims = await userManager.GetClaimsAsync(applicationUser);

                    var roleClaims = (await Task.WhenAll(userRoles.Select(async role =>
                        await roleManager.GetClaimsAsync(await roleManager.FindByNameAsync(role)))))
                        .Where(c => c != null)
                        .SelectMany(c => c)
                        .ToList();

                    var allClaims = userClaims.Concat(roleClaims).ToList();

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
