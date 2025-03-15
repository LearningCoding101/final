using BLL.DTO.Course;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class EditCourseViewModel
    {
        public CourseDto Course { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Lecturers { get; set; }
    }
}