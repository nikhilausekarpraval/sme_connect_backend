using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Dtos
{
    public class TaskItemsDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateOnly? AssignedOnDt { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? CreatedOnDt { get; set; }
        public string? CreatedBy { get; set; }
        public int? EmployeeId { get; set; }
    }
}
