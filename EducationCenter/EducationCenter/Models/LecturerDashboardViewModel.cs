using BLL.DTO.Course;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class LecturerDashboardViewModel
    {
        public IEnumerable<CourseListDto> Courses { get; set; }
    }
}