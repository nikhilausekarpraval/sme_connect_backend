﻿using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IGroupUserProvider
    {
        public Task<ApiResponse<bool>> AddGroupUser(GroupUserRequestDto userGroupUser);

        public Task<ApiResponse<bool>> DeleteGroupUser(List<int> ids);

        public Task<ApiResponse<bool>> UpdateGroupUser(GroupUserRequestDto group);

        public Task<ApiResponse<List<GroupUser>>> GetUserGroups(string userEmail,string practice);

        public Task<ApiResponse<List<GetGroupUsersWithRoleClaims>>> getGroupAllUsers(string group);

        public Task<ApiResponse<List<GetGroupUsersWithRoleClaims>>> getLeadUserGroupsUsers(string userEmail);

        public Task<ApiResponse<bool>> getIsUserLeadForGroups(string userEmail);

        public  Task<ApiResponse<List<GetGroupUsersWithRoleClaims>>> getGroupUsers();
    }
}
