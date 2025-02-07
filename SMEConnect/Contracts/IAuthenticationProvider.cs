using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Helpers;
using SMEConnect.Modals.JWTAuthentication.Authentication;
using SMEConnect.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static SMEConnect.Constatns.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SMEConnect.Contracts
{
    public interface IAuthenticationProvider
    {
        public  Task<ResponseDto> Login(LoginModalDto model);

        public  Task<ResponseDto> Register(RegisterModelDto model);

        public Task<ResponseDto> Logout();

        public  Task<ResponseDto> UpdateUser(RegisterModelDto user);

        public  Task<ResponseDto> ResetPassword(ResetPasswordDto user);

        public  Task<ResponseDto> ForgetPassword(ResetPasswordDto user);

        public  Task<ResponseDto> RegisterAdmin(RegisterModelDto model);

    }
}
