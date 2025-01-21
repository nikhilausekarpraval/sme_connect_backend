
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver;
using SMEConnectSignalRServer.AppContext;
using SMEConnectSignalRServer.Dtos;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Modals;
using SMEConnectSignalRServer.Models;

namespace SMEConnectSignalRServer.Services
{
    public class MessageService : IMessageService
    {
        private SMEConnectSignalRServerContext _sMEConnectSignalRServerContext1;
        private ILogger<MessageService> _logger;
        private readonly IHubContext<ChatHub, IChatClient> _hubContext;

        public MessageService(SMEConnectSignalRServerContext sMEConnectSignalRServerContext,ILogger<MessageService> logger, IHubContext<ChatHub, IChatClient> hubContext)
        {
            _sMEConnectSignalRServerContext1 = sMEConnectSignalRServerContext;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<List<Message>> GetDiscussionChat(UserDto userDto)
        {
            try
            {
                var filter = Builders<Message>.Filter.And(
                    Builders<Message>.Filter.Eq(m => m.Practice, userDto.Practice),
                    Builders<Message>.Filter.Eq(m => m.Discussion, userDto.Discussion),
                    Builders<Message>.Filter.Eq(m => m.Group, userDto.Group)
                );

                var result = await _sMEConnectSignalRServerContext1.Messages.Find(filter).ToListAsync();
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> AddMessage(Message message)
        {
            try
            {
                //first we are brodcasting message to all users
                await _hubContext.Clients.All.ReceiveMessage(message);

                // saving message into db
                await _sMEConnectSignalRServerContext1.Messages.InsertOneAsync(message);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

        public async Task<long> DeleteMessagesByFilter(List<int> messageIds)
        {
            try
            {
                var filter = Builders<Message>.Filter.In(m => m.Id, messageIds);
                var result = await _sMEConnectSignalRServerContext1.Messages.DeleteManyAsync(filter);
                return result.DeletedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }


        public async Task<Message> UpdateMessage(Message message)
        {
            try
            {
                var filter = Builders<Message>.Filter.Eq(m => m.Id, message.Id);
                var update = Builders<Message>.Update
                    .Set(m => m.Text, message.Text)
                    .Set(m => m.ReplyedTo, message.ReplyedTo);

                var result = await _sMEConnectSignalRServerContext1.Messages.FindOneAndUpdateAsync(filter, update);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(1, $"{ex.Message}", ex);
                throw;
            }
        }

    }
}
