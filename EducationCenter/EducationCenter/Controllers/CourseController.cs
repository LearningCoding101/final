using BLL.DTO.Course;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;

        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
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

    }
}
