using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using SMEConnect.Contracts;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Helpers
{
    public class Helper
    {
        public static byte[] HashString(string answer)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(answer));
            }
        }

        public static string GetErrorEntityWithIdNotFound(string entity, int id)
        {
            return $"{entity} with ID {id} not found.";
        }

        public static dynamic GetAuthToken(List<Claim> authClaims, IConfiguration _configuration)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            return new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Audience"],
                            expires: DateTime.Now.AddHours(72), // Token expiration time
                            claims: authClaims, // All claims (roles + custom claims + role claims)
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
        }

        public static dynamic GetApplicationUser(RegisterModelDto model)
        {
            return new ApplicationUser()
            {
                Email = model.email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.userName,
                DisplayName = model.displayName,
                PhoneNumber = model.phoneNumber,
                Practice = model.practice,
                ModifiedOnDt = DateTime.Now,
            };

        }

        public static List<string> GetQuestion(RegisterModelDto model)
        {
            return new List<string>
                {
                    model.question1,
                    model.question2,
                    model.question3
                };
        }

        public static List<string> GetAnswers(RegisterModelDto model)
        {
            return new List<string>
                 {
                    model.answer1,
                    model.answer2,
                    model.answer3
                };
        }

        public static string GetAccessToken(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
            }
            return null; 
        }

        public static StringContent GetSerializedData<T>(T data)
        {
           return new StringContent(
                JsonSerializer.Serialize(data),
                Encoding.UTF8,
                "application/json");
        }

        public static void AddAuthorizationHeader(HttpClient httpClient, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        public static async Task<bool> IsGroupLeadAsync(IAuthenticationProvider authenticationProvider, UserContext userContext)
        {
            if (userContext.Roles.Contains("Admin"))
            {
                var groupRole = await authenticationProvider.GetUserGroupRole(userContext.Email);
                return groupRole == GroupRoles.Lead;
            }
            return false;
        }
    }
}

