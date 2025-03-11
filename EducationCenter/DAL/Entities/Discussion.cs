using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Discussion
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }

        // Relationships
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

}
