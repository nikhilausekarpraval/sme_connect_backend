using SMEConnect.Contracts;
using Microsoft.AspNetCore.Identity;

namespace SMEConnect.Modals
{

    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }
    }

}
