using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Dtos
{
    public class TaskItemsDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime? assignedOnDt { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime? createdOnDt { get; set; }
        public string createdBy { get; set; }
        public int? employeeId { get; set; }
    }
}
