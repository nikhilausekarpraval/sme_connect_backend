using Microsoft.AspNetCore.Identity;

namespace SMEConnect.Modals
{
    public class ApplicationRole : IdentityRole
    {
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOnDt { get; set; }
    }
}
