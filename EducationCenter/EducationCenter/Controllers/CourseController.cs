using BLL.DTO.Course;
using BLL.DTO.Enrollment;
using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationCenter.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, IEnrollmentService enrollmentService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View(courses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }
        [Authorize(Roles = "Admin,Lecturer")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Create(CreateCourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return View(courseDto);
            }

            var result = await _courseService.CreateCourseAsync(courseDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unsucessfull");
                return View(courseDto);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Edit(int id, UpdateCourseDto courseDto)
        {
            if (!ModelState.IsValid)
            {
                return View(courseDto);
            }

            var result = await _courseService.UpdateCourseAsync(id, courseDto);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unsuccessful");
                return View(courseDto);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _courseService.DeleteCourseAsync(id);
            if (!result)
            {
                return BadRequest(result);
            }

            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public async Task<IActionResult> TrainingPrograms()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var categories = await _courseService.GetAllCategoriesAsync();
            var viewModel = new TrainingProgramsViewModel
            {
                Courses = courses,
                Categories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _enrollmentService.EnrollStudentAsync(new CreateEnrollmentDto
            {
                CourseId = courseId,
                UserId = Int32.Parse(userId)
            });
            if (result == null)
            {
                TempData["Error"] = "Unsuccessful enrollment";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            return RedirectToAction("MyCourses", "Enrollment");
        }



    }

}
