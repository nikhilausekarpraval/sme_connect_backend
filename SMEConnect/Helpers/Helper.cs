﻿using Microsoft.IdentityModel.Tokens;
using SMEConnect.Dtos;
using SMEConnect.Modals;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
    }
}

