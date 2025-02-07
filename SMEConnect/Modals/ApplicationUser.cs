using Microsoft.AspNetCore.Identity;
using SMEConnect.Dtos;

namespace SMEConnect.Modals
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public string? Practice { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }

        public ICollection<RoleDto>? Roles { get; set; }

        public ICollection<UserClaimDto>? Claims { get; set; }
    }
}
