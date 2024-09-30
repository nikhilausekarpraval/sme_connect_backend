using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DemoDotNetCoreApplication.Modals
{
    [Table("employee")]
    public class Employee
    {
        public int id { get; set; }  
        public string name { get; set; }
        public string position { get; set; }
        public string designation { get; set; }
        public string email { get; set; }

        [Column("mobile_no")]
        public string mobileNo { get; set; }

        [Column("created_on_dt")]
        public DateTime? createdOnDt { get; set; }

        [Column("created_by")]
        public string createdBy { get; set; }
        [JsonIgnore]
        public List<TaskItem>? tasks { get; set; }
    }
}
