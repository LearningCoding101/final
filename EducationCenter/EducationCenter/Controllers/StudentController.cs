using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EducationCenter.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseMaterialService _courseMaterialService;
        private readonly IEnrollmentService _enrollmentService;
        
        public StudentController(ICourseService courseService, ICourseMaterialService courseMaterialService, IEnrollmentService enrollmentService)
        {
            _courseService = courseService;
            _courseMaterialService = courseMaterialService;
            _enrollmentService = enrollmentService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var courses = await _courseService.GetAllCoursesAsync();
            var enrollments = await _enrollmentService.GetEnrollmentsByUserAsync(int.Parse(userId));
            var courseMaterials = await _courseMaterialService.GetAllMaterialsAsync();
            var courseCategory = await _courseService.GetAllCategoriesAsync();
            var viewModel = new StudentDashboardViewModel
            {
                Courses = courses,
                Enrollments = enrollments,
                CourseMaterials = courseMaterials,
                Categories = courseCategory
            };

            return View(viewModel);
        }
    }
}