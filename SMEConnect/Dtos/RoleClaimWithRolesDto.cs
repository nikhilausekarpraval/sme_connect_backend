namespace SMEConnect.Dtos
{
    public class RoleClaimWithRolesDto
    {
        public int? id { get; set; }

        public string? claimType { get; set; }

        public string? claimValue { get; set; }

        public List<RoleDto> roles { get; set; } = new List<RoleDto>();
    }
}
