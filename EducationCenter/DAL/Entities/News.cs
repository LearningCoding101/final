using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string Type { get; set; } // News / Event
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventDate { get; set; }
        public string? EventLocation { get; set; }
    }

}
