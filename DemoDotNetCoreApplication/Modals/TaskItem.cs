using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DemoDotNetCoreApplication.Modals
{
    [Table("task")]
    public class TaskItem
    {
        public int id { get; set; }  
        public string name { get; set; }
        public string description { get; set; }
        [Column("assigned_on_dt")]

        public DateTime? assignedOnDt { get; set; }
        [Column("end_date")]
        public DateTime? endDate { get; set; }
        [Column("created_on_dt")]
        public DateTime? createdOnDt { get; set; }

        [Column("created_by")]
        public string createdBy { get; set; }

        [ForeignKey("employee")]
        [Column("employee_id")]
        public int? employeeId { get; set; }

        public Employee? employee { get; set; }
    }
}
