using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Discussion
{
    public class UpdateDiscussionDto
    {
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(2000)]
        public string? Content { get; set; }
    }
}
