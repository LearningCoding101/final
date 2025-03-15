using BLL.DTO.Course;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class CreateCourseViewModel
    {
        public CreateCourseDto Course { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Lecturers { get; set; }
        public bool IsLecturerUser { get; set; }

    }
}