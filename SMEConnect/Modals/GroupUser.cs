namespace SMEConnect.Modals
{
    public class GroupUser
    {
        public int Id { get; set; }

        public string? Group { get; set; }

        public string? GroupRole { get; set; }

        public string? UserEmail { get; set; }

        public DateTime? ModifiedOnDt { get; set; }

        public string? ModifiedBy { get; set; }
    }
}
