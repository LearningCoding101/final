using BLL.DTO.Course;
using BLL.DTO.Enrollment;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class StatisticsViewModel
    {
        public string Period { get; set; }
        public string PeriodName { get; set; }
        public int EnrollmentCount { get; set; }
        public int CompletedCourses { get; set; }
        public decimal CompletionRate { get; set; }
        public List<CourseListDto> TopCourses { get; set; }
        public List<EnrollmentDto> RecentEnrollments { get; set; }
    }
}