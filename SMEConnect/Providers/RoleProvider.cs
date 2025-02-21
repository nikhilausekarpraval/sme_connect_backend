using Microsoft.AspNetCore.Identity;
using SMEConnect.Modals;
using Task = System.Threading.Tasks.Task;

namespace SMEConnect.Providers
{
    public class RoleProvider
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roleNames = { "Admin", "SuperAdmin", "Manager", "SME", "Lead","LPU"};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }
        }
    }
}
