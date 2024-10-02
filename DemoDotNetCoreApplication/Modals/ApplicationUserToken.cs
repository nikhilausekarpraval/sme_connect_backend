using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser? User { get; set; }
    }
}
