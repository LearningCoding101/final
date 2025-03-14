using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EducationCenter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly INewsService _newsService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ICourseService courseService, INewsService newsService, ILogger<HomeController> logger)
        {
            _courseService = courseService;
            _newsService = newsService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var popularCourses = await _courseService.GetAllCoursesAsync();
            var latestNews = await _newsService.GetAllNewsAsync();
            var viewModel = new HomeViewModel
            {
                PopularCourses = popularCourses,
                LatestNews = latestNews
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}