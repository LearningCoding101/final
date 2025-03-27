using BLL.DTO.Course;
using BLL.DTO.Enrollment;
using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EducationCenter.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;

        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<CourseController> _logger;

        public CourseController(ICourseService courseService, IUserService userService, IEnrollmentService enrollmentService, ILogger<CourseController> logger)
        {
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _userService = userService;

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
        public async Task<IActionResult> Create()
        {
            var categories = await _courseService.GetAllCategoriesAsync();
            var lecturers = (await _userService.GetAllUserAsync())
                                .Where(u => u.Role == "Lecturer" && u.IsActive)
                                .ToList();

            var viewModel = new Models.CreateCourseViewModel
            {
                Course = new CreateCourseDto(),
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),
                Lecturers = lecturers.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.FullName
                }),
                IsLecturerUser = User.IsInRole("Lecturer")
            };

            if (viewModel.IsLecturerUser)
            {
                int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                viewModel.Course.LecturerId = currentUserId;
            }

            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Create(CreateCourseViewModel model)
        {
            // if (!ModelState.IsValid)
            // {
            //     // Re-populate lists
            //     var categories = await _courseService.GetAllCategoriesAsync();
            //     var lecturers = (await _userService.GetAllUserAsync())
            //                         .Where(u => u.Role == "Lecturer" && u.IsActive)
            //                         .ToList();

            //     model.Categories = categories.Select(c => new SelectListItem
            //     {
            //         Value = c.Id.ToString(),
            //         Text = c.Name
            //     });
            //     model.Lecturers = lecturers.Select(l => new SelectListItem
            //     {
            //         Value = l.Id.ToString(),
            //         Text = l.FullName
            //     });
            //     return View(model);
            // }
            if (User.IsInRole("Lecturer"))
            {
                int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                model.Course.LecturerId = currentUserId;
                model.IsLecturerUser = true;
            }
            var result = await _courseService.CreateCourseAsync(model.Course);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unsuccessful");

                // Re-populate lists
                var categories = await _courseService.GetAllCategoriesAsync();
                var lecturers = (await _userService.GetAllUserAsync())
                                    .Where(u => u.Role == "Lecturer" && u.IsActive)
                                    .ToList();

                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                model.Lecturers = lecturers.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.FullName
                });
                return View(model);
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

            var categories = await _courseService.GetAllCategoriesAsync();
            var lecturers = (await _userService.GetAllUserAsync())
                                .Where(u => u.Role == "Lecturer" && u.IsActive)
                                .ToList();

            var viewModel = new Models.EditCourseViewModel
            {
                Course = course,
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }),
                Lecturers = lecturers.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.FullName
                })
            };

            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Lecturer")]
        public async Task<IActionResult> Edit(EditCourseViewModel model)
        {
            /*            if (!ModelState.IsValid)
                        {
                            // Re-populate lists
                            var categories = await _courseService.GetAllCategoriesAsync();
                            var lecturers = (await _userService.GetAllUserAsync())
                                                    .Where(u => u.Role == "Lecturer" && u.IsActive)
                                                    .ToList();

                            model.Categories = categories.Select(c => new SelectListItem
                            {
                                Value = c.Id.ToString(),
                                Text = c.Name
                            });
                            model.Lecturers = lecturers.Select(l => new SelectListItem
                            {
                                Value = l.Id.ToString(),
                                Text = l.FullName
                            });
                            return View(model);
                        }
            */
            // Map model.Course to UpdateCourseDto
            var courseDto = new UpdateCourseDto
            {
                /*                Id = model.Course.Id,
                */
                Title = model.Course.Title,
                Description = model.Course.Description,
                Type = model.Course.Type,
                Price = model.Course.Price,
                StartDate = model.Course.StartDate,
                EndDate = model.Course.EndDate,
                CategoryId = model.Course.CategoryId,
                LecturerId = model.Course.LecturerId
            };

            var result = await _courseService.UpdateCourseAsync(model.Course.Id, courseDto);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Unsuccessful update.");

                var categories = await _courseService.GetAllCategoriesAsync();
                var lecturers = (await _userService.GetAllUserAsync())
                                        .Where(u => u.Role == "Lecturer" && u.IsActive)
                                        .ToList();

                model.Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                model.Lecturers = lecturers.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.FullName
                });
                return View(model);
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
            try
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

            }
            catch(InvalidOperationException ex)
            {
                TempData["Error"] = "Already enrolled duh";
            }
            return RedirectToAction("Dashboard", "Student");
        }



    }

}
