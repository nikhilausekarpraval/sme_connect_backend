namespace DemoDotNetCoreApplication.Dtos
{
    public class EmployeeDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string designation { get; set; }
        public string email { get; set; }

        public string mobileNo { get; set; }
        public DateTime? createdOnDt { get; set; }
        public string createdBy { get; set; }
    }
}
