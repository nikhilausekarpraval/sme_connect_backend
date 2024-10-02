using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{

        public class ApplicationUserRole : IdentityUserRole<string>
        {
            public virtual ApplicationUser? User { get; set; }
            public virtual ApplicationRole? Role { get; set; }
        }
    
}
