using BLL.DTO.Course;
using BLL.DTO.News;

namespace EducationCenter.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CourseListDto> PopularCourses { get; set; }
        public IEnumerable<NewsDto> LatestNews { get; set; }
    }
}
