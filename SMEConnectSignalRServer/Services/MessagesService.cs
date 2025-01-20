﻿
using MongoDB.Driver;
using SMEConnectSignalRServer.AppContext;
using SMEConnectSignalRServer.Dtos;
using SMEConnectSignalRServer.Interfaces;
using SMEConnectSignalRServer.Modals;

namespace SMEConnectSignalRServer.Services
{
    public class MessagesService : IMessageService
    {
        private SMEConnectSignalRServerContext _sMEConnectSignalRServerContext1;
        private readonly ILogger _logger;

        public MessagesService(SMEConnectSignalRServerContext sMEConnectSignalRServerContext, ILogger logger) {
                _sMEConnectSignalRServerContext1 = sMEConnectSignalRServerContext;
            _logger = logger;
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
                return result;
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
