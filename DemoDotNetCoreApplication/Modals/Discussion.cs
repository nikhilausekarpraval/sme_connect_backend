
using static DemoDotNetCoreApplication.Constatns.Constants;

namespace DemoDotNetCoreApplication.Modals
{
    public class Discussion
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DiscussionStatus Status { get; set; }
        public string? GroupName { get; set; }
        public DateTime? ModifiedOnDt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
