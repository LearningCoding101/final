using BLL.DTO.CourseMaterial;
using BLL.DTO.Discussion;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class CourseStudyViewModel
    {
        public int EnrollmentId { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int Progress { get; set; }
        public string LecturerName { get; set; }
        public IEnumerable<CourseMaterialDto> Materials { get; set; }
        public IEnumerable<DiscussionDto> Discussions { get; set; }
    }
}