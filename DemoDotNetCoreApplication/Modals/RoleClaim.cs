using DemoDotNetCoreApplication.Contracts;
using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{

    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }
    }

}
