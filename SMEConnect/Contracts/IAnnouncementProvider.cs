using SMEConnect.Dtos;
using SMEConnect.Modals;

namespace SMEConnect.Contracts
{
    public interface IAnnouncementProvider
    {
        public Task<ApiResponse<List<Announcement>>> getUsersLast5DaysAnnouncements(UserDetailsDto userDetailsDto);

        public Task<ApiResponse<bool>> AddAnnouncement(Announcement announcement);
    }
}
