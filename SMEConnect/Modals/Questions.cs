using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SMEConnect.Modals
{
    public class Questions
    {
        public string? question {  get; set; }
        public byte[] answerHash { get; set; }
        public int id { get; set; }
        public string? user_id { get; set; }
    }
}
