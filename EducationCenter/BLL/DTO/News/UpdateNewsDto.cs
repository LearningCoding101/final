using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.News
{
    public class UpdateNewsDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime? EventDate { get; set; }
        public string? EventLocation { get; set; }
    }
}
