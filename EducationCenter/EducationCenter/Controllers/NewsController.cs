using BLL.DTO.News;
using BLL.Interface;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.Controllers
{

    [Authorize(Roles = "Admin")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(INewsService newsService, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetAllNewsAsync();
            return View(news);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var newsItem = await _newsService.GetNewsByIdAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(newsDto);
            }

            var result = await _newsService.CreateNewsAsync(newsDto);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unsucessful");
                return View(newsDto);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _newsService.DeleteNewsAsync(id);
            if (!result)
            {
                return BadRequest("Unsucessful");
            }

            return RedirectToAction("Index");
        }
    }
}
