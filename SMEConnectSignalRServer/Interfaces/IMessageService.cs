﻿
using SMEConnectSignalRServer.Dtos;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Interfaces
{
    public interface IMessageService
    {
        public Task<List<Message>> GetDiscussionChat(UserDto userDto);

        public Task<bool> AddMessage(Message message);

        public Task<long> DeleteMessagesByFilter(List<dynamic> messageIds);

        public Task<List<string>> GetRecentDiscussionsFromGroups(DiscussionsDTO userDto);

        public Task<List<string>> GetSimilarDiscussions(DiscussionsDTO discussionsDTO);

        public Task<List<string>> GetDiscussionsUsers(DiscussionsDTO discussionsDTO);

        public Task<Message> UpdateMessage(Message message);

    }
}
