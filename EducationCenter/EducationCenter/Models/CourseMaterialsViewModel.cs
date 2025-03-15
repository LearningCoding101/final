using BLL.DTO.CourseMaterial;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class CourseMaterialsViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public IEnumerable<CourseMaterialDto> Materials { get; set; }
    }
}