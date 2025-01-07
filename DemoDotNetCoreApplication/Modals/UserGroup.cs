namespace DemoDotNetCoreApplication.Modals
{
    public partial class UserGroup
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string ? Description { get; set; }

        public DateOnly? ModifiedOnDt { get; set; }

        public string? ModifiedBy { get; set; }

    }
}
