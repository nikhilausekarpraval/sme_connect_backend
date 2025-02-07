namespace SMEConnect.Dtos
{
    public class AddClaimToUserDto
    {
        public int? id { get; set; }
        public string? userId { get; set; }
        public string? claimType { get; set; }
        public string? claimValue { get; set; }
    }
}
