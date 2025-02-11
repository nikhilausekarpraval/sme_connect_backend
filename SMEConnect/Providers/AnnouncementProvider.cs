using Microsoft.EntityFrameworkCore;
using SMEConnect.Constatns;
using SMEConnect.Contracts;
using SMEConnect.Data;
using SMEConnect.Dtos;
using SMEConnect.Modals;


namespace SMEConnect.Providers
{
    public class AnnouncementProvider : IAnnouncementProvider
    {
        private readonly DcimDevContext _context;
        private readonly ILogger<AnnouncementProvider> _logger;

        public AnnouncementProvider(DcimDevContext context, ILogger<AnnouncementProvider> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<List<Announcement>>> getUsersLast5DaysAnnouncements(UserDetailsDto userDetailsDto)
        {
            try
            {
                DateTime lastFiveDays = DateTime.UtcNow.AddDays(-5);

                var query = _context.Announcements
                    .Where(a => a.CreatedAt >= lastFiveDays); 

                if (!userDetailsDto.IsAdmin) 
                {
                    query = query.Where(a => a.PracticeName == userDetailsDto.PracticeName);
                }

                var announcements = await query.ToListAsync();

                return new ApiResponse<List<Announcement>>(Constants.ApiResponseType.Success, announcements, "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(1, ex, ex.Message);
                return new ApiResponse<List<Announcement>>(Constants.ApiResponseType.Failure, null, ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> AddAnnouncement(Announcement announcement)
        {
            try
            {
                await _context.Announcements.AddAsync(announcement);
                await _context.SaveChangesAsync();
                return new ApiResponse<bool>(Constants.ApiResponseType.Success, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(1, ex, ex.Message);
                return new ApiResponse<bool>(Constants.ApiResponseType.Failure, false, ex.Message);
            }
        }
    }
}
