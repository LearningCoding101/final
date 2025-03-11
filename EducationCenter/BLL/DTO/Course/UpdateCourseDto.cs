using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Course
{
    public class UpdateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public decimal? Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get; set; }
        public int? LecturerId { get; set; }
    }
}
