using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser? User { get; set; }
    }
}
