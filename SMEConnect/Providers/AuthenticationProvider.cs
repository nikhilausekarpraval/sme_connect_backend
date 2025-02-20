using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Helpers;
using SMEConnect.Modals;
using SMEConnect.Modals.JWTAuthentication.Authentication;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Providers
{
    public class AuthenticationProvider : IAuthenticationProvider
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly DcimDevContext _dcimDevContext;
        private readonly ILogger<AuthenticationProvider> _logger;


        public AuthenticationProvider(UserManager<ApplicationUser> userManager, ILogger<AuthenticationProvider> logger, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, DcimDevContext dcimDevContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            this.signInManager = signInManager;
            _dcimDevContext = dcimDevContext;
            this._logger = logger;

        }

        public async Task<ResponseDto> GetUserContext(string userEmail)
        {
            try
            {
                string sqlQuery = Query.selectUsers;
                var emailParam = new SqlParameter("@email", userEmail);
                var user = await _dcimDevContext.Users.FromSqlRaw(sqlQuery, emailParam).FirstAsync(); ;

                var newname = user.DisplayName;
                IList<string> gUserRoles;
                IList<Claim> gUserClaims;
                IList<Claim>? gRoleClaims = null;

                    var userRoles = await userManager.GetRolesAsync(user);
                    gUserRoles = userRoles;

                    var userClaims = await userManager.GetClaimsAsync(user);
                    gUserClaims = userClaims;

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Email, user?.Email),

                    };

                    var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProvider>());
                    var mapper = config.CreateMapper();

                    // Mapping string roles to RoleDto
                    IList<RoleDto> roleDtos = mapper.Map<IList<RoleDto>>(gUserRoles);
                    user.Roles = roleDtos;

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                        var role = await roleManager.FindByNameAsync(userRole);

                        var roleClaims = await roleManager.GetClaimsAsync(role);

                        foreach (var roleClaim in roleClaims)
                        {
                            authClaims.Add(roleClaim);
                            gRoleClaims = roleClaims;
                        }

                    }

                    foreach (var userClaim in userClaims)
                    {
                        authClaims.Add(userClaim);
                    }

                    UserContextDto userContextDto = new UserContextDto { User = user, Roles = gUserRoles, UserClaims = gUserClaims, RoleClaims = gRoleClaims };

                    var accessToken = new
                    {
                        token ="",
                        expiration = "",
                        userContext = userContextDto
                    };

                return new ResponseDto { status = ApiResponseType.Success, statusText = "", message = "", data = accessToken };
                
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<string> GetUserGroupRole(string userEmail)
        {
            try
            {
                var userGroupRole = await (from u in _dcimDevContext.Users
                                           join gu in _dcimDevContext.GroupUsers on u.Email equals gu.UserEmail
                                           join ug in _dcimDevContext.UserGroups on gu.Group equals ug.Name
                                           where u.Email == userEmail && ug.Practice == u.Practice
                                           select gu.GroupRole)
                                          .FirstOrDefaultAsync();

                return userGroupRole;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<ResponseDto> Login(LoginModalDto model)
        {
            // var user = await userManager.FindByEmailAsync(model.UserName);
            try
            {
                string sqlQuery = Query.selectUsers;
                var emailParam = new SqlParameter("@email", model.UserName);
                var user = await _dcimDevContext.Users.FromSqlRaw(sqlQuery, emailParam).FirstAsync(); ;

                var newname = user.DisplayName;
                IList<string> gUserRoles;
                IList<Claim> gUserClaims;
                IList<Claim>? gRoleClaims = null;

                if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);
                    gUserRoles = userRoles;

                    var userClaims = await userManager.GetClaimsAsync(user);
                    gUserClaims = userClaims;

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Email, user?.Email),

                    };

                    var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProvider>());
                    var mapper = config.CreateMapper();

                    // Mapping string roles to RoleDto
                    IList<RoleDto> roleDtos = mapper.Map<IList<RoleDto>>(gUserRoles);
                    user.Roles = roleDtos;

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                        var role = await roleManager.FindByNameAsync(userRole);

                        var roleClaims = await roleManager.GetClaimsAsync(role);

                        foreach (var roleClaim in roleClaims)
                        {
                            authClaims.Add(roleClaim);
                            gRoleClaims = roleClaims;
                        }

                    }

                    foreach (var userClaim in userClaims)
                    {
                        authClaims.Add(userClaim);
                    }

                    var token = Helper.GetAuthToken(authClaims, _configuration);

                    UserContextDto userContextDto = new UserContextDto { User = user, Roles = gUserRoles, UserClaims = gUserClaims, RoleClaims = gRoleClaims };
                    var accessToken = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        userContext = userContextDto
                    };

                    return new ResponseDto { status = ApiResponseType.Success, statusText = "", message = "", data = accessToken };
                }

                return new ResponseDto { status = ApiResponseType.Failure, statusText = AccessConfigurationErrorMessage.WrongEmailPassword, message = AccessConfigurationErrorMessage.WrongEmailPassword, data = null };

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }


        public async Task<ResponseDto> Register(RegisterModelDto model,string userEmail)
        {
            try
            {
                var userExists = await userManager.FindByEmailAsync(model.email);

                if (userExists != null)
                    return new ResponseDto { status = ApiResponseType.NotFound, statusText = ApiErrors.DuplicateEmailOrUser, message = "" };

                ApplicationUser user = Helper.GetApplicationUser(model);
                user.ModifiedBy = userEmail;
                var result = await userManager.CreateAsync(user, model.password);

                if (!result.Succeeded)
                {
                    return new ResponseDto { status = ApiResponseType.Failure, statusText = ApiErrors.ErrorCreatingUser, message = result?.Errors?.FirstOrDefault().Description };
                }

                var newUser = await userManager.FindByEmailAsync(model.email);
                var questions = Helper.GetQuestion(model);
                var answers = Helper.GetAnswers(model);

                await _dcimDevContext.Questions.AddRangeAsync(questions.Select((q, i) => new Questions
                {
                    question = q,
                    answerHash = Helper.HashString(answers[i]),
                    user_id = newUser.Id
                }));

                await AddRolesAndClaimsToUser(model, newUser);

                await _dcimDevContext.SaveChangesAsync();

                return new ResponseDto { status = ApiResponseType.Success, statusText = AccessConfigurationSccessMessage.UserCreated, message = result.ToString() };

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<ResponseDto> Logout()
        {
            try
            {
                await signInManager.SignOutAsync();

                return new ResponseDto { status = ApiResponseType.Success, statusText = AccessConfigurationSccessMessage.UserLoggedOut, message = "" };
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }

        public async Task<bool> AddRolesAndClaimsToUser(RegisterModelDto model, ApplicationUser newUser)
        {

            // Add new roles
            if (model.roles != null && model.roles.Any())
            {
                var roleResult = await userManager.AddToRolesAsync(newUser, model.roles);
                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add roles: {errors}");
                }
            }

            // Add new claims
            var userClaims = model.claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            if (userClaims.Any())
            {
                var claimsResult = await userManager.AddClaimsAsync(newUser, userClaims);
                if (!claimsResult.Succeeded)
                {
                    var errors = string.Join(", ", claimsResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to add claims: {errors}");
                }
            }

            return true;
        }

        // update who can access this method currently can done without password
        public async Task<ResponseDto> UpdateUser(RegisterModelDto user,string userEmail)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(user.email);
                if (currentUser != null && await userManager.CheckPasswordAsync(currentUser, user?.password ?? "") || true)
                {
                    currentUser.DisplayName = user.displayName;
                    currentUser.UserName = user.userName;
                    currentUser.PhoneNumber = user.phoneNumber;
                    currentUser.Practice = user.practice;
                    currentUser.ModifiedBy = userEmail;
                    currentUser.ModifiedOnDt = DateTime.Now;

                    var result = await userManager.UpdateAsync(currentUser);

                    // Remove existing claims
                    var currentClaims = await userManager.GetClaimsAsync(currentUser);
                    foreach (var claim in currentClaims)
                    {
                        var removeClaimResult = await userManager.RemoveClaimAsync(currentUser, claim);
                        if (!removeClaimResult.Succeeded)
                        {
                            throw new Exception($"Failed to remove claim: {claim.Type}");
                        }
                    }

                    // Remove existing roles
                    var currentRoles = await userManager.GetRolesAsync(currentUser);
                    var removeRolesResult = await userManager.RemoveFromRolesAsync(currentUser, currentRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        // Handle error
                        var errors = string.Join(", ", removeRolesResult.Errors.Select(e => e.Description));
                        throw new Exception($"Failed to remove roles: {errors}");
                    }

                    await AddRolesAndClaimsToUser(user, currentUser);

                    await _dcimDevContext.SaveChangesAsync();

                    return new ResponseDto { status = ApiResponseType.Success, statusText = AccessConfigurationSccessMessage.UpdatedUser, message = "", data = result };

                }
                else
                {
                    return new ResponseDto { status = ApiResponseType.Failure, statusText = AccessConfigurationErrorMessage.FaildToUpdate, message = "", };
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ResponseDto> ResetPassword(ResetPasswordDto user)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(user.email);

                if (currentUser != null)
                {
                    if (await userManager.CheckPasswordAsync(currentUser, user.password))
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(currentUser);
                        var passwordResetResult = await userManager.ResetPasswordAsync(currentUser, token, user.newPassword);

                        if (passwordResetResult.Succeeded)
                        {
                            currentUser.DisplayName = user.displayName;
                            currentUser.UserName = user.userName;
                            currentUser.Email = user.email;

                            var updateResult = await userManager.UpdateAsync(currentUser);

                            if (updateResult.Succeeded)
                            {
                                return new ResponseDto { message = "", status = ApiResponseType.Success, statusText = AccessConfigurationSccessMessage.PasswordUserUpdated };
                            }
                            else
                            {
                                return new ResponseDto { statusText = AccessConfigurationErrorMessage.FaildToUpdate, status = ApiResponseType.Failure, message = "" };
                            }
                        }
                        else
                        {
                            return new ResponseDto { statusText = AccessConfigurationErrorMessage.FaildToResetPassword, status = ApiResponseType.Failure, message = "" };
                        }
                    }
                    else
                    {
                        return new ResponseDto { statusText = AccessConfigurationErrorMessage.OldPasswordIncorrect, status = ApiResponseType.Failure, message = "" };
                    }
                }
                else
                {
                    throw new Exception(AccessConfigurationErrorMessage.UserNotFound);
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }


        public async Task<ResponseDto> ForgetPassword(ResetPasswordDto user)
        {
            try
            {
                var currentUser = await userManager.FindByEmailAsync(user.email);

                if (currentUser != null)
                {

                    var hashedAnswer = Helper.HashString(user.answer1);
                    var userQuestion = await _dcimDevContext.Questions
                       .FirstOrDefaultAsync(q => q.user_id == currentUser.Id
                                                 && q.question == user.question1
                                                 && q.answerHash.SequenceEqual(hashedAnswer));

                    if (userQuestion != null)
                    {
                        var token = await userManager.GeneratePasswordResetTokenAsync(currentUser);
                        var passwordResetResult = await userManager.ResetPasswordAsync(currentUser, token, user.password);

                        if (passwordResetResult.Succeeded)
                        {

                            currentUser.DisplayName = user.displayName;
                            currentUser.UserName = user.userName;
                            currentUser.Email = user.email;

                            var updateResult = await userManager.UpdateAsync(currentUser);

                            return updateResult.Succeeded ?

                                 new ResponseDto { statusText = AccessConfigurationSccessMessage.PasswordUserUpdated, status = ApiResponseType.Success, message = "" }
                            :
                                 new ResponseDto { statusText = AccessConfigurationErrorMessage.FaildToUpdateUser, status = ApiResponseType.Failure, message = "" };

                        }
                        else
                        {
                            return new ResponseDto { statusText = AccessConfigurationErrorMessage.FaildToResetPassword, status = ApiResponseType.Failure, message = "" };
                        }
                    }
                    else
                    {
                        return new ResponseDto { statusText = AccessConfigurationErrorMessage.QuestionOrAnswerIsWrong, status = ApiResponseType.Failure, message = "" };
                    }
                }

                else
                {
                    throw new Exception(AccessConfigurationErrorMessage.UserNotFound);
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }

        }


        public async Task<ResponseDto> RegisterAdmin(RegisterModelDto model)
        {
            try
            {
                var userExists = await userManager.FindByNameAsync(model.userName);
                if (userExists != null)
                    return new ResponseDto { status = ApiResponseType.Failure, message = AccessConfigurationErrorMessage.UserAlreadyExist };

                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.userName
                };

                var result = await userManager.CreateAsync(user, model.password);
                if (!result.Succeeded)
                    return new ResponseDto { status = ApiResponseType.Failure, message = AccessConfigurationErrorMessage.UserCreationFaild };

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(getApplicationUser(UserRoles.Admin));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }

                return new ResponseDto { status = ApiResponseType.Success, message = AccessConfigurationSccessMessage.UserCreated };

            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;

            }

        }

        public ApplicationRole getApplicationUser(string id)
        {
            return new ApplicationRole()
            {
                Name = id,
                ModifiedBy = ""
            };
        }
    }
}
