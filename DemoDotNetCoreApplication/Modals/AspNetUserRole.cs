using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class AspNetUserRole : IdentityUserRole<string>
    {

        public virtual AspNetUser User { get; set; }
        public virtual AspNetRole Role { get; set; }
    }

}
