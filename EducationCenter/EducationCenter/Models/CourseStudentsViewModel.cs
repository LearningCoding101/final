using BLL.DTO.Enrollment;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class CourseStudentsViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public IEnumerable<EnrollmentDto> Enrollments { get; set; }
    }
}