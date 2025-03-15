using BLL.DTO.CourseMaterial;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class StudentMaterialsViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public int EnrollmentId { get; set; }
        public IEnumerable<CourseMaterialDto> Materials { get; set; }
        public int CurrentProgress { get; set; }
    }
}