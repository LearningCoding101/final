using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationCenter.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class LecturerController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseMaterialService _courseMaterialService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IDiscussionService _discussionService;

        public LecturerController(
            ICourseService courseService,
            ICourseMaterialService courseMaterialService,
            IEnrollmentService enrollmentService,
            IDiscussionService discussionService)
        {
            _courseService = courseService;
            _courseMaterialService = courseMaterialService;
            _enrollmentService = enrollmentService;
            _discussionService = discussionService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await _courseService.GetCoursesByLecturerAsync(userId);

            var viewModel = new LecturerDashboardViewModel
            {
                Courses = courses
            };

            return View(viewModel);
        }

        public async Task<IActionResult> MyCourses()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var courses = await _courseService.GetCoursesByLecturerAsync(userId);

            return View(courses);
        }

        public async Task<IActionResult> Students(int courseId)
        {
            var enrollments = await _enrollmentService.GetEnrollmentsByCourseAsync(courseId);
            var course = await _courseService.GetCourseByIdAsync(courseId);

            var viewModel = new CourseStudentsViewModel
            {
                CourseId = courseId,
                CourseTitle = course?.Title ?? "Unknown Course",
                Enrollments = enrollments
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Materials(int courseId)
        {
            var materials = await _courseMaterialService.GetMaterialsByCourseAsync(courseId);
            var course = await _courseService.GetCourseByIdAsync(courseId);

            var viewModel = new CourseMaterialsViewModel
            {
                CourseId = courseId,
                CourseTitle = course?.Title ?? "Unknown Course",
                Materials = materials
            };

            return View(viewModel);
        }
    }
}