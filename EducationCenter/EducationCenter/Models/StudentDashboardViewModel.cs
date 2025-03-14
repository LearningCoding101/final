using BLL.DTO.Course;
using BLL.DTO.CourseCategory;
using BLL.DTO.CourseMaterial;
using BLL.DTO.Enrollment;

namespace EducationCenter.Models
{
    public class StudentDashboardViewModel
    {
        public IEnumerable<CourseListDto> Courses { get; set; }
        public IEnumerable<CourseMaterialDto> CourseMaterials { get; set; }
        public IEnumerable<EnrollmentDto> Enrollments { get; set; }
        public IEnumerable<CourseCategoryDto> Categories { get; set; }

    }
}
