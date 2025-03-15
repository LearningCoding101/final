using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Interface;
using EducationCenter.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using BLL.DTO.CourseMaterial;

namespace EducationCenter.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICourseMaterialService _courseMaterialService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IDiscussionService _discussionService;
        private readonly IUserService _userService;
        public StudentController(
            ICourseService courseService,
            IUserService userService,
            ICourseMaterialService courseMaterialService,
            IEnrollmentService enrollmentService,
            IDiscussionService discussionService)
        {
            _courseService = courseService;
            _courseMaterialService = courseMaterialService;
            _enrollmentService = enrollmentService;
            _userService = userService;
            _discussionService = discussionService;
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
        public async Task<IActionResult> CourseStudy(int enrollmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);

            if (enrollment == null || enrollment.UserId != userId)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseByIdAsync(enrollment.CourseId);
            var materials = await _courseMaterialService.GetMaterialsByCourseAsync(enrollment.CourseId) ?? Enumerable.Empty<CourseMaterialDto>();
            var discussions = await _discussionService.GetDiscussionsByCourseAsync(enrollment.CourseId);
            var lecturer = await _userService.GetUserByIdAsync(course.LecturerId);
            var viewModel = new CourseStudyViewModel
            {
                EnrollmentId = enrollment.Id,
                CourseId = course.Id,
                CourseTitle = course.Title,
                CourseDescription = course.Description,
                Progress = enrollment.Progress,
                LecturerName = lecturer.FullName,
                Materials = materials,
                Discussions = discussions
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Materials(int courseId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Get enrollments for this user
            var enrollments = await _enrollmentService.GetEnrollmentsByUserAsync(userId);

            // Find the specific enrollment for this course
            var enrollment = enrollments.FirstOrDefault(e => e.CourseId == courseId);

            if (enrollment == null)
            {
                return NotFound("You are not enrolled in this course");
            }

            var course = await _courseService.GetCourseByIdAsync(courseId);
            var materials = await _courseMaterialService.GetMaterialsByCourseAsync(courseId);

            var viewModel = new StudentMaterialsViewModel
            {
                CourseId = courseId,
                CourseTitle = course.Title,
                EnrollmentId = enrollment.Id,
                Materials = materials,
                CurrentProgress = enrollment.Progress
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Progress(int enrollmentId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);

            if (enrollment == null || enrollment.UserId != userId)
            {
                return NotFound();
            }

            var course = await _courseService.GetCourseByIdAsync(enrollment.CourseId);
            var materials = await _courseMaterialService.GetMaterialsByCourseAsync(enrollment.CourseId);
            var materialProgressItems = materials.Select(m => new MaterialProgressItem
            {
                MaterialId = m.Id,
                MaterialTitle = m.Title,
                IsCompleted = false // Would come from a student progress tracking table in a real implementation
            }).ToList();

            var viewModel = new StudentProgressViewModel
            {
                EnrollmentId = enrollment.Id,
                CourseId = course.Id,
                CourseTitle = course.Title,
                CurrentProgress = enrollment.Progress,
                MaterialProgresses = materialProgressItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int enrollmentId, int progress)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);

            if (enrollment == null || enrollment.UserId != userId)
            {
                return NotFound();
            }

            // Update the enrollment progress
            await _enrollmentService.UpdateProgressAsync(enrollmentId, progress);

            return RedirectToAction("CourseStudy", new { enrollmentId });
        }
    }
}