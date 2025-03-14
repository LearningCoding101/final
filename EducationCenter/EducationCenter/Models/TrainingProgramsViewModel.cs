using BLL.DTO.Course;
using BLL.DTO.CourseCategory;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class TrainingProgramsViewModel
    {
        public IEnumerable<CourseListDto> Courses { get; set; }
        public IEnumerable<CourseCategoryDto> Categories { get; set; }
    }
}