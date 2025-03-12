using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.News
{
    public class CreateNewsDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Type { get; set; } // "News" or "Event"

        public string ThumbnailUrl { get; set; }

        public DateTime? EventDate { get; set; } // Only for events
        public string EventLocation { get; set; } // Only for events
    }

}
