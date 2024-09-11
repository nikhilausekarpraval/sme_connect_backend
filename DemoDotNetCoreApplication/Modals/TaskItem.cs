using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DemoDotNetCoreApplication.Modals
{
    public class TaskItem
    {
        public int Id { get; set; }  
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AssignedOnDt { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOnDt { get; set; }
        public string CreatedBy { get; set; }
        public int employeeId { get; set; }
        public Employee employee { get; set; }
    }
}
