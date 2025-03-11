using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public Discussion Discussion { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
