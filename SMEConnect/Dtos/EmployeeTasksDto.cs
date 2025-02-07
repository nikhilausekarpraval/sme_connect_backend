using System.ComponentModel.DataAnnotations.Schema;

namespace SMEConnect.Dtos
{
    public class EmployeeTasksDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Designation { get; set; }
        public string? Email { get; set; }

        public string? MobileNo { get; set; }

        public DateOnly? CreatedOnDt { get; set; }

        public string? CreatedBy { get; set; }

        public virtual ICollection<TaskItemsDto> Tasks { get; set; } = new List<TaskItemsDto>();
    }
}
