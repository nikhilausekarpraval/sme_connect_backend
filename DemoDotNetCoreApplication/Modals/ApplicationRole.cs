namespace DemoDotNetCoreApplication.Modals
{
    using System;
    using System.Transactions;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
