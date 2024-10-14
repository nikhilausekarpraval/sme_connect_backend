namespace DemoDotNetCoreApplication.Dtos
{
    public class AddClaimToRoleDto
    {
        public string roleName { get; set; }
        public string claimType { get; set; }
        public string claimValue { get; set; }
    }
}
