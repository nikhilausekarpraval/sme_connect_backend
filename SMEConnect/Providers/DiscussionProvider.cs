using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.IO;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Modals;
using SMEConnectSignalRServer.Dtos;
using System.Text.Json;
using System.Text;
using SMEConnect.Dtos;


namespace SMEConnect.Providers
{

    public class DiscussionProvider : IDiscussionProvider
    {
        private readonly DcimDevContext _context;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        private readonly IUserContext _userContext;

        public DiscussionProvider(DcimDevContext context, ILogger<DiscussionProvider> logger, HttpClient httpClient,IUserContext userContext)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
            _userContext = userContext;
        }

        public async Task<ApiResponse<List<Discussion>>> getDiscussions(string groupId)
        {
            try
            {
                var discussion = await _context.Discussions.Where(d => d.GroupName == groupId).ToListAsync();
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussion, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> DeleteDiscussion(string discussionsIds)
        {
            try
            {
                var discussionsToRemove = _context.Discussions.Where(discussion => discussion.Name == discussionsIds).ToList();

                if (discussionsToRemove.Any())
                {
                    _context.Discussions.RemoveRange(discussionsToRemove);
                    await _context.SaveChangesAsync();
                    return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
                }
                else
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "No matching discussion found.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }


        public async Task<ApiResponse<List<Discussion>>> GetRecentDiscussions(DiscussionsDTO discussion)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(discussion), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5234/api/message/get-recent-discussions", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, "Failed to fetch recent discussions");
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper<List<string>>>(responseContent);
                var discussionNames = apiResponse?.Value ?? new List<string>();

                if (discussionNames == null || !discussionNames.Any())
                {
                    return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, new List<Discussion>(), "No discussions found.");
                }

                var discussions = await _context.Discussions
                    .Where(d => discussionNames.Contains(d.Name))
                    .ToListAsync();

                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussions, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recent discussions.");
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<List<Discussion>>> GetSimilarDiscussionsFromGroup(DiscussionsDTO discussion)
        {
            try {

                var newDiscussion = await (from d in _context.Discussions
                                         join gu in _context.GroupUsers on d.GroupName equals gu.Group
                                         join g in _context.UserGroups on gu.Group equals g.Name
                                         where d.GroupName == discussion.Group
                                            && g.Practice == discussion.Practice
                                            && gu.UserEmail == _userContext.Email
                                         select d)
                                .FirstOrDefaultAsync();

                discussion.Discussion = newDiscussion?.Description;

                var jsonContent = new StringContent(JsonSerializer.Serialize(discussion), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5234/api/message/get-similar-discussions", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, "Failed to fetch similar discussions.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper<List<string>>>(responseContent);
                var recentDiscussionNames = apiResponse?.Value ?? new List<string>();

                if (recentDiscussionNames == null || !recentDiscussionNames.Any())
                {
                    return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, new List<Discussion>(), "No similar discussions found.");
                }

                var discussions = await _context.Discussions
                    .Where(d => recentDiscussionNames.Contains(d.Name) && d.GroupName == discussion.Group)
                    .ToListAsync();

                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Success, discussions, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching similar discussions.");
                return new ApiResponse<List<Discussion>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<List<GroupUserDto>>> GetDiscussionUsers(DiscussionsDTO discussion)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(discussion), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("http://localhost:5234/api/message/get-discussion-users", jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<List<GroupUserDto>>(Constants.ApiResponseType.Failure, null, "Failed to fetch discussion users.");
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponseWrapper<List<string>>>(responseContent);
                var userNames = apiResponse?.Value ?? new List<string>();

                if (userNames == null || !userNames.Any())
                {
                    return new ApiResponse<List<GroupUserDto>>(Constants.ApiResponseType.Success, new List<GroupUserDto>(), "No users found.");
                }

                var usersWithNames = await _context.GroupUsers
                                   .Where(gu => gu.Group == discussion.Group && userNames.Contains(gu.UserEmail))
                                   .Join(
                                       _context.Users,
                                       gu => gu.UserEmail,
                                       u => u.Email,
                                       (gu, u) => new
                                       {
                                           gu.Id,
                                           gu.Group,
                                           gu.UserEmail,
                                           u.DisplayName,
                                           gu.GroupRole,
                                           gu.ModifiedOnDt,
                                           gu.ModifiedBy
                                       })
                                   .ToListAsync();

                var userDtos = usersWithNames.Select(u => new GroupUserDto
                {
                    Id = u.Id,
                    Group = u.Group,
                    UserEmail = u.UserEmail,
                    Name = u.DisplayName,
                    GroupRole = u.GroupRole,
                    ModifiedBy = u.ModifiedBy,
                    ModifiedOnDt = u.ModifiedOnDt

                }).ToList();

                return new ApiResponse<List<GroupUserDto>>(Constants.ApiResponseType.Success, userDtos, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching discussion users.");
                return new ApiResponse<List<GroupUserDto>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }


        public async Task<ApiResponse<bool>> CreateDiscussion(Discussion discussion)
        {
            try
            {
                bool exists = await _context.Discussions.AnyAsync(p => p.Name == discussion.Name);
                if (exists)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "A discussion with the same name already exists.");
                }

                await _context.Discussions.AddAsync(discussion);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateDiscussion(Discussion discussion)
        {
            try
            {
                var existingdiscussion = await _context.Discussions.FindAsync(discussion.Name);
                if (existingdiscussion == null)
                {
                    return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, "Selected discussion not found.");
                }

                existingdiscussion.Name = discussion.Name;
                existingdiscussion.Description = discussion.Description;
                existingdiscussion.Status = discussion.Status;

                _context.Discussions.Update(existingdiscussion);
                await _context.SaveChangesAsync();
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
