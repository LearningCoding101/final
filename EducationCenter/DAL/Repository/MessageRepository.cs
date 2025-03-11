using DAL.Data;
using DAL.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    internal class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(Context context) : base(context) { }

        public async Task<IEnumerable<Message>> GetMessagesByDiscussionIdAsync(int discussionId)
        {
            return await _context.Messages.Where(m => m.DiscussionId == discussionId).ToListAsync();
        }
    }
}
