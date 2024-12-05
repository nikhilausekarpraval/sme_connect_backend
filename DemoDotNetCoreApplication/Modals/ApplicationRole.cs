using Microsoft.AspNetCore.Identity;

namespace DemoDotNetCoreApplication.Modals
{
    public class ApplicationRole : IdentityRole
    {
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }
    }
}
