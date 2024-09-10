namespace DemoDotNetCoreApplication.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public DateTime CreatedOnDt { get; set; }
        public string CreatedBy { get; set; }
    }
}
