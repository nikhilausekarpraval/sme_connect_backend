using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public int? practice_id {  get; set; }
    }
}
