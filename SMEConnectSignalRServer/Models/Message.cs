using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SMEConnectSignalRServer.Modals
{
    public class Message
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string? Text { get; set; }

        public string? UserName { get; set; }

        public string? DisplayName { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public List<FileAttachment>? Attachments { get; set; } = new List<FileAttachment>();

        public string? ReplyedTo { get; set; }

        public string? Discussion { get; set; }

        public string? Group { get; set; }

        public string? Practice { get; set; }
    }
}
