using BLL.DTO.Enrollment;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EducationCenter.Controllers
{

    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly ILogger<EnrollmentController> _logger;

        public EnrollmentController(IEnrollmentService enrollmentService, ILogger<EnrollmentController> logger)
        {
            _enrollmentService = enrollmentService;
            _logger = logger;
        }

        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var enrollments = await _enrollmentService.GetEnrollmentsByUserAsync(Int32.Parse(userId));
            return View(enrollments);
        }

        [HttpPost]
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
                TempData["Error"] = "Unsucessful enrollment";
                return RedirectToAction("Details", "Course", new { id = courseId });
            }

            return RedirectToAction("MyCourses");
        }

        public async Task<IActionResult> Progress(int enrollmentId)
        {
            var enrollment = await _enrollmentService.GetEnrollmentByIdAsync(enrollmentId);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        [HttpPost]
        public async Task<IActionResult> Unenroll(int enrollmentId)
        {
            var result = await _enrollmentService.DeleteEnrollmentAsync(enrollmentId);
            if (!result)
            {
                return BadRequest("Unsuccessful");
            }

            return RedirectToAction("MyCourses");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(int enrollmentId, int progress, string paymentStatus, int courseId)
        {
            // Create update DTO with the form values
            var updateDto = new UpdateEnrollmentDto
            {
                Progress = progress,
                PaymentStatus = paymentStatus
            };

            // Update the enrollment
            var result = await _enrollmentService.UpdateEnrollmentAsync(enrollmentId, updateDto);

            if (!result)
            {
                TempData["Error"] = "Failed to update student progress.";
            }
            else
            {
                TempData["Success"] = "Student progress updated successfully.";
            }

            // Redirect back to the Students view for the course
            return RedirectToAction("Students", "Lecturer", new { courseId });
        }

    }
}
