namespace SMEConnectSignalRServer.Dtos
{
    public class MessageDto
    {
        public string? Text { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Discussion { get; set; }
        public string? ReplyedTo { get; set; }
        public string? Group { get; set; }
        public string? Practice { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }

}
