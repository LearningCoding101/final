using BLL.DTO.Discussion;
using BLL.Interface;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationCenter.Controllers
{
    [Authorize]
    public class DiscussionController : Controller
    {
        private readonly IDiscussionService _discussionService;
        private readonly ILogger<DiscussionController> _logger;

        public DiscussionController(IDiscussionService discussionService, ILogger<DiscussionController> logger)
        {
            _discussionService = discussionService;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int courseId)
        {
            var discussions = await _discussionService.GetDiscussionsByCourseAsync(courseId);
            return View(discussions);
        }

        public async Task<IActionResult> Details(int discussionId)
        {
            var discussion = await _discussionService.GetDiscussionByIdAsync(discussionId);
            if (discussion == null)
            {
                return NotFound();
            }
            return View(discussion);
        }
        [Authorize(Roles = "Student,Lecturer")]
        public IActionResult Create(Guid courseId)
        {
            ViewBag.CourseId = courseId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Student,Lecturer")]
        public async Task<IActionResult> Create(CreateDiscussionDto discussionDto)
        {
            if (!ModelState.IsValid)
            {
                return View(discussionDto);
            }

            discussionDto.UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _discussionService.CreateDiscussionAsync(discussionDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unsucessful");
                return View(discussionDto);
            }

            return RedirectToAction("Index", new { courseId = discussionDto.CourseId });
        }

        
        [Authorize(Roles = "Admin,Lecturer")]
        [HttpPost]
        public async Task<IActionResult> Delete(int discussionId, int courseId)
        {
            var result = await _discussionService.DeleteDiscussionAsync(discussionId);
            if (!result)
            {
                return BadRequest("Unsucessful");
            }

            return RedirectToAction("Index", new { courseId });
        }
    }

}
