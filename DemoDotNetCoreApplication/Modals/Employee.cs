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
        public string mobile_no { get; set; }
        public DateTime created_on_dt { get; set; }
        public string created_by { get; set; }
        [JsonIgnore]
        public List<TaskItem> task_items { get; set; }
    }
}
