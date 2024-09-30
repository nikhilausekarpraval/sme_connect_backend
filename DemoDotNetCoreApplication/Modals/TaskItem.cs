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
        public DateTime assigned_on_dt { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on_dt { get; set; }
        public string created_by { get; set; }

        [ForeignKey("employee")]
        public int? employee_id { get; set; }
        public Employee employee { get; set; }
    }
}
