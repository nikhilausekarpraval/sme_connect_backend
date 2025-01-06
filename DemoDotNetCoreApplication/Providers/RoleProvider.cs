using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Task = System.Threading.Tasks.Task;

namespace DemoDotNetCoreApplication.Providers
{
    public class RoleProvider
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roleNames = { "Admin", "User","Manager","SME","Lead" };
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
