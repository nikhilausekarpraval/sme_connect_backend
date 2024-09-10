using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Dtos
{
    public class TaskItemsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AssignedOnDt { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedOnDt { get; set; }
        public string CreatedBy { get; set; }

        public int Employee_id { get; set; }
    }
}
