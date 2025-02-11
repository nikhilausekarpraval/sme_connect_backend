namespace SMEConnect.Modals
{
    public class Announcement
    {
        public int Id { get; set; }
        public string? UserName { get; set; } 
        public string GroupName { get; set; }
        public string PracticeName { get; set; }
        public bool IsFeature { get; set; }
        public bool IsAdmin { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public bool IsRead { get; set; }
    }
}
