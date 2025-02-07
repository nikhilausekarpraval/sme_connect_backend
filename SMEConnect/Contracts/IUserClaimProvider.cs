using Microsoft.AspNetCore.Identity;
using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IUserClaimProvider
    {
        public Task<List<IdentityUserClaim<string>>> GetUserClaims();


        public Task<ApiResponse<string>> DeleteUserClaims(List<int> UserClaimIds);


        public Task<ApiResponse<string>> UpdateUserClaims(List<UserClaimDto> UserClaimDtos);


        public Task<string> AddClaimToUser(List<AddClaimToUserDto> userClaim);

    }
}
