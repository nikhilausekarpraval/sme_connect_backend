using SMEConnect.Modals;
using System.Security.Claims;

namespace SMEConnect.Dtos
{
    public class UserContextDto
    {
        public ApplicationUser? User { get; set; }
        public IList<string>? Roles { get; set; }
        public IList<Claim>? UserClaims { get; set; }
        public IList<Claim>? RoleClaims { get; set; }

    }
}
