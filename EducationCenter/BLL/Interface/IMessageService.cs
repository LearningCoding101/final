using BLL.DTO.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetMessagesByDiscussionAsync(int discussionId);
        Task<MessageDto?> GetMessageByIdAsync(int id);
        Task<MessageDto> CreateMessageAsync(CreateMessageDto messageDto);
        Task<bool> UpdateMessageAsync(int id, int userId, UpdateMessageDto messageDto);
        Task<bool> DeleteMessageAsync(int id, int userId);
    }
}
