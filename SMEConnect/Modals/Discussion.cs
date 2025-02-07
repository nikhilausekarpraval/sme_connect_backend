
using System.ComponentModel.DataAnnotations;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Modals
{
    public class Discussion
    {
        [Key]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? GroupName { get; set; }
        public DateTime? ModifiedOnDt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
