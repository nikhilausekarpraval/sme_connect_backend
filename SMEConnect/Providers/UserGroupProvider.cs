﻿using Microsoft.EntityFrameworkCore;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;
using static SMEConnect.Constatns.Constants;

namespace SMEConnect.Providers
{
    public class UserGroupProvider : IGroupProvider
    {
        private ILogger<UserGroupProvider> _logger;
        private DcimDevContext _dcimDevContext;
        private IUserContext _userContext;

        public UserGroupProvider(ILogger<UserGroupProvider> Logger, DcimDevContext dcimDevContext, IUserContext userContext)
        {
            this._dcimDevContext = dcimDevContext;
            this._logger = Logger;
            _userContext = userContext;
        }


        public async Task<ApiResponse<bool>> AddGroup(UserGroup group)
        {
            try
            {
                bool exists = await _dcimDevContext.UserGroups.AnyAsync(p => p.Name == group.Name);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A group with the same name already exists.");
                }

                await _dcimDevContext.UserGroups.AddAsync(group);
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserGroup>>> getGroups()
        {
            try
            {
                List<UserGroup> roles = await _dcimDevContext.UserGroups.ToListAsync();
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<List<UserGroup>>(ApiResponseType.Success, roles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<List<UserGroup>>> getUserPracticeGroups(string practice = "")
        {
            try
            {
                var user = await _dcimDevContext.Users.FirstOrDefaultAsync(u => u.Email == _userContext.Email);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var practiceToUse = !string.IsNullOrEmpty(practice) ? practice : user.Practice;

                List<UserGroup> roles = await _dcimDevContext.UserGroups.Where(g => g.Practice == practiceToUse).ToListAsync();

                return new ApiResponse<List<UserGroup>>(ApiResponseType.Success, roles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }



        public async Task<ApiResponse<bool>> DeleteUserGroup(List<int> ids)
        {
            try
            {
                var userGroupsToRemove = _dcimDevContext.UserGroups.Where(userGroup => ids.Contains(userGroup.Id)).ToList();

                if (userGroupsToRemove.Any())
                {
                    _dcimDevContext.UserGroups.RemoveRange(userGroupsToRemove);
                    await _dcimDevContext.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching user groups found.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

        public async Task<ApiResponse<bool>> UpdateGroup(UserGroup group)
        {
            try
            {
                var existingGroup = await _dcimDevContext.UserGroups.FindAsync(group.Id);
                if (existingGroup == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected group not found.");
                }

                existingGroup.Name = group.Name;
                existingGroup.Description = group.Description;
                existingGroup.Practice = group.Practice;

                _dcimDevContext.UserGroups.Update(existingGroup);
                await _dcimDevContext.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                throw;
            }
        }

    }

}

