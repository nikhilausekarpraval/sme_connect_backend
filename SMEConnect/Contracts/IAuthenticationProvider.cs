using DemoDotNetCoreApplication.Data;
using DemoDotNetCoreApplication.Dtos;
using DemoDotNetCoreApplication.Helpers;
using DemoDotNetCoreApplication.Modals.JWTAuthentication.Authentication;
using DemoDotNetCoreApplication.Modals;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using static DemoDotNetCoreApplication.Constatns.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DemoDotNetCoreApplication.Contracts
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
