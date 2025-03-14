using BLL.DTO.User;
using BLL.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationCenter.Controllers
{

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        [AllowAnonymous]

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var authResponse = await _userService.AuthenticateAsync(loginDto);
            if (authResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginDto);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authResponse.User.Id.ToString()),
                new Claim(ClaimTypes.Name, authResponse.User.Email),
                new Claim(ClaimTypes.Role, authResponse.User.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = authResponse.Expiration
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
            if (authResponse.User.Role == "Student")
            {
                return RedirectToAction("Dashboard", "Student");
            }
            return RedirectToAction("Index", "Home");
        }


        /*        [Authorize(Roles = "Admin")]
        */
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUserAsync();
            return View(users);
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            var user = await _userService.GetUserByIdAsync(Int32.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            return View(user);

        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            var result = await _userService.RegisterAsync(userDto);
            if (result != null)
            {
                /*ModelState.AddModelError(string.Empty, result.Message);*/
                return View(userDto);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", updateDto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _userService.UpdateUserAsync(Int32.Parse(userId), updateDto);
            if (result != null)
            {
                return View("Profile", updateDto);
            }

            return RedirectToAction("Profile");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return BadRequest("Not Successful");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
