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

        public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var email = context.User.Claims.FirstOrDefault(c => c.Type.Contains("emailaddress"))?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    var applicationUser = await userManager.FindByEmailAsync(email);

                    if (applicationUser != null)
                    {
                        var userContext = new UserContext
                        {
                            Name = applicationUser?.UserName,
                            Email = applicationUser?.Email,
                            UserName = applicationUser?.UserName
                        };

                        context.Items["UserContext"] = userContext;
                    }
                }
            }

            await _next(context);
        }
    }
}
