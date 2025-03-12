using BLL.DTO.Message;
using BLL.Interface;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MessageDto> CreateMessageAsync(CreateMessageDto messageDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(messageDto.UserId);
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(messageDto.DiscussionId);

            if (user == null || discussion == null)
            {
                throw new InvalidOperationException("User or Discussion not found.");
            }

            var message = new Message
            {
                DiscussionId = messageDto.DiscussionId,
                UserId = messageDto.UserId,
                Content = messageDto.Content,
                Timestamp = DateTime.UtcNow
            };

            await _unitOfWork.Messages.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return new MessageDto
            {
                Id = message.Id,
                DiscussionId = message.DiscussionId,
                UserId = message.UserId,
                UserName = (await _unitOfWork.Users.GetByIdAsync(message.UserId))?.FullName ?? "Unknown",
                Content = message.Content,
                Timestamp = message.Timestamp
            };
        }


        public async Task<bool> DeleteMessageAsync(int id, int userId)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(id);
            if (message == null || message.UserId != userId) return false;

            _unitOfWork.Messages.Delete(message);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<MessageDto?> GetMessageByIdAsync(int id)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(id);
            if (message == null) return null;

            return new MessageDto
            {
                Id = message.Id,
                DiscussionId = message.DiscussionId,
                UserId = message.UserId,
                UserName = message.User.FullName,
                Content = message.Content,
                Timestamp = message.Timestamp
            };
        }
        public async Task<IEnumerable<MessageDto>> GetMessagesByDiscussionAsync(int discussionId)
        {
            var messages = await _unitOfWork.Messages.GetMessagesByDiscussionIdAsync(discussionId);

            return messages.Select(m => new MessageDto
            {
                Id = m.Id,
                DiscussionId = m.DiscussionId,
                UserId = m.UserId,
                UserName = m.User.FullName,
                Content = m.Content,
                Timestamp = m.Timestamp
            });
        }

        public async Task<bool> UpdateMessageAsync(int id, int userId, UpdateMessageDto messageDto)
        {
            var message = await _unitOfWork.Messages.GetByIdAsync(id);
            if (message == null || message.UserId != userId) return false;

            message.Content = messageDto.Content;
            _unitOfWork.Messages.Update(message);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
