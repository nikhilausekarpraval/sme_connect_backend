using SMEConnect.Dtos;
using SMEConnect.Modals.JWTAuthentication.Authentication;

namespace SMEConnect.Contracts
{
    public interface IAuthenticationProvider
    {
        public Task<ResponseDto> Login(LoginModalDto model);

        public Task<ResponseDto> Register(RegisterModelDto model, string userEmail);

        public Task<ResponseDto> Logout();

        public Task<ResponseDto> GetUserContext(string userEmail);

        public Task<ResponseDto> UpdateUser(RegisterModelDto user,string userEmail);

        public Task<ResponseDto> ResetPassword(ResetPasswordDto user);

        public Task<ResponseDto> ForgetPassword(ResetPasswordDto user);

        public Task<ResponseDto> RegisterAdmin(RegisterModelDto model);

        public  Task<string> GetUserGroupRole(string userEmail);

    }
}
