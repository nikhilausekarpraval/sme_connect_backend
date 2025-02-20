
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMEConnect.Modals
{
    public class GroupRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool RequestStatus { get; set; }

        [Required]
        [MaxLength(255)]
        public string RequestRole { get; set; }

        [Required]
        [MaxLength(255)]
        public string GroupName { get; set; }

        [Required]
        [MaxLength(255)]
        public string PracticeName { get; set; }

        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        public bool ApprovalStatus { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ModifiedOnDt { get; set; } = DateTime.UtcNow;

        [MaxLength(255)]
        public string? ModifiedBy { get; set; }
    }

}
