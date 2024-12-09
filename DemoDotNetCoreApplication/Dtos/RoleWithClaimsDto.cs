using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Dtos
{
    public class RoleWithClaimsDto
    {
        public string? Id  {get; set; }

        public string? Name { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }

        public List<IdentityRoleClaim<string>>? Claims { get; set; }
    }
}
