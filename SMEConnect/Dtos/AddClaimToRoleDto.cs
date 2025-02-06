namespace DemoDotNetCoreApplication.Dtos
{
    public class AddClaimToRoleDto
    {
        public int? id {  get; set; }
        public string? roleId { get; set; }
        public string? claimType { get; set; }
        public string? claimValue { get; set; }
    }
}
