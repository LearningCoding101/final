using BLL.DTO.CourseMaterial;
using BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class CourseMaterialController : Controller
    {
        private readonly ICourseMaterialService _courseMaterialService;
        private readonly ILogger<CourseMaterialController> _logger;

        public CourseMaterialController(ICourseMaterialService courseMaterialService, ILogger<CourseMaterialController> logger)
        {
            _courseMaterialService = courseMaterialService;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int courseId)
        {
            var materials = await _courseMaterialService.GetMaterialByIdAsync(courseId);
            return View(materials);
        }

        public IActionResult Upload(Guid courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(CreateCourseMaterialDto materialDto)
        {
            if (!ModelState.IsValid)
            {
                return View(materialDto);
            }

            var result = await _courseMaterialService.AddMaterialAsync(materialDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unsucessful");
                return View(materialDto);
            }

            return RedirectToAction("Index", new { courseId = materialDto.CourseId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int materialId, int courseId)
        {
            var result = await _courseMaterialService.DeleteMaterialAsync(materialId);
            if (!result)
            {
                return BadRequest("Unsucessful");
            }

            return RedirectToAction("Index", new { courseId });
        }
    }
}
