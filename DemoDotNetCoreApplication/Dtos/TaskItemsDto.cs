using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Dtos
{
    public class TaskItemsDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime assigned_on_dt { get; set; }
        public DateTime end_date { get; set; }
        public DateTime created_on_dt { get; set; }
        public string created_by { get; set; }
        public int employee_id { get; set; }
    }
}
