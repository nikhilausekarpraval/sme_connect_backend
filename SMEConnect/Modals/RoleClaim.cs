using Microsoft.AspNetCore.Identity;
using SMEConnect.Contracts;

namespace SMEConnect.Modals
{

    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }
    }

}
