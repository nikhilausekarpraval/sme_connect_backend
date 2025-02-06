using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Authorization;

namespace DemoDotNetCoreApplication.Providers
{
    public class CustomClaimHandlerProvider : AuthorizationHandler<CustomClaimRequirement>
    {
        protected override System.Threading.Tasks.Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomClaimRequirement requirement)
        {
            var claim = context.User.FindFirst(c => c.Type == requirement.ClaimType);
            if (claim != null && claim.Value.Equals(requirement.ClaimValue, StringComparison.OrdinalIgnoreCase))
            {
                context.Succeed(requirement);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    }

}
