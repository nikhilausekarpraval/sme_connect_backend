using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DemoDotNetCoreApplication.Modals;

namespace DemoDotNetCoreApplication.Dtos
{
    public class UserContextDto
    {
            public ApplicationUser? User { get; set; }
            public IList<string>? Roles { get; set; }
            public IList<Claim>? UserClaims { get; set; }
            public IList<Claim>? RoleClaims { get; set; }
        
    }
}
