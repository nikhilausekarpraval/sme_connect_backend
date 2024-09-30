namespace DemoDotNetCoreApplication.Dtos
{
    public class EmployeeDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string designation { get; set; }
        public string email { get; set; }
        public string mobile_no { get; set; }
        public DateTime created_on_dt { get; set; }
        public string created_by { get; set; }
    }
}
