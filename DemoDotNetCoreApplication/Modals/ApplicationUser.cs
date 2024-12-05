using DemoDotNetCoreApplication.Dtos;
using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class ApplicationUser : IdentityUser
    {
        public string? DisplayName { get; set; }

        public int? Practice_id {  get; set; }

        public int? Groups_id { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }

        public ICollection<RoleDto> Roles { get; set; }
    }
}
