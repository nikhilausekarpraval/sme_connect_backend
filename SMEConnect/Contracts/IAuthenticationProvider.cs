using SMEConnect.Dtos;
using SMEConnect.Modals.JWTAuthentication.Authentication;

namespace SMEConnect.Contracts
{
    public interface IAuthenticationProvider
    {
        public Task<ResponseDto> Login(LoginModalDto model);

        public Task<ResponseDto> Register(RegisterModelDto model);

        public Task<ResponseDto> Logout();

        public Task<ResponseDto> UpdateUser(RegisterModelDto user);

        public Task<ResponseDto> ResetPassword(ResetPasswordDto user);

        public Task<ResponseDto> ForgetPassword(ResetPasswordDto user);

        public Task<ResponseDto> RegisterAdmin(RegisterModelDto model);

    }
}
