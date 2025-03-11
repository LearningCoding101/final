using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Course
{
    public class CourseListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; } // Online / Offline
        public decimal Price { get; set; }
        public string LecturerName { get; set; }
        public string CategoryName { get; set; }
    }
}
