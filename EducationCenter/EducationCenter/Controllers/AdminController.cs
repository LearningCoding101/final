using BLL.DTO.User;
using BLL.Interface;
using EducationCenter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationCenter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICourseService _courseService;
        private readonly IEnrollmentService _enrollmentService;
        private readonly INewsService _newsService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUserService userService,
            ICourseService courseService,
            IEnrollmentService enrollmentService,
            INewsService newsService,
            ILogger<AdminController> logger)
        {
            _userService = userService;
            _courseService = courseService;
            _enrollmentService = enrollmentService;
            _newsService = newsService;
            _logger = logger;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _userService.GetAllUserAsync();
            var courses = await _courseService.GetAllCoursesAsync();
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            // Calculate total revenue
            decimal totalRevenue = enrollments
                .Where(e => e.PaymentStatus == "Paid")
                .Sum(e => courses.FirstOrDefault(c => c.Id == e.CourseId)?.Price ?? 0);

            // Create sample revenue data for chart
            var lastSixMonths = Enumerable.Range(0, 6)
                .Select(i => DateTime.Now.AddMonths(-i).ToString("MMM yyyy"))
                .Reverse()
                .ToList();

            var randomRevenue = new List<decimal>();
            var rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                randomRevenue.Add(Math.Round((decimal)rnd.Next(1000, 5000) + rnd.Next(0, 100) / 100M, 2));
            }

            // Create sample activity data
            var activities = new List<RecentActivityDto>();
            var actions = new[] { "Created course", "Updated user", "Enrolled", "Completed course" };
            var userList = users.ToList();
            for (int i = 0; i < 5; i++)
            {
                activities.Add(new RecentActivityDto
                {
                    UserName = userList[rnd.Next(userList.Count)].FullName,
                    Action = actions[rnd.Next(actions.Length)],
                    Detail = $"Action detail #{i + 1}",
                    Timestamp = DateTime.Now.AddHours(-i * 4)
                });
            }

            var viewModel = new AdminDashboardViewModel
            {
                TotalUsers = users.Count(),
                StudentCount = users.Count(u => u.Role == "Student"),
                LecturerCount = users.Count(u => u.Role == "Lecturer"),
                AdminCount = users.Count(u => u.Role == "Admin"),
                ActiveCourses = courses.Count(),
                TotalRevenue = totalRevenue,
                ActiveEnrollments = enrollments.Count(e => e.Progress < 100),
                RevenueMonths = lastSixMonths,
                RevenueValues = randomRevenue,
                RecentActivities = activities
            };

            return View(viewModel);
        }

        public async Task<IActionResult> UserManagement(string searchTerm = null, string roleFilter = null, string statusFilter = null)
        {
            var users = await _userService.GetAllUserAsync();

            // Apply filters if provided
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                users = users.Where(u =>
                    u.Email.ToLower().Contains(searchTerm) ||
                    u.FullName.ToLower().Contains(searchTerm)
                ).ToList();
            }

            if (!string.IsNullOrEmpty(roleFilter))
            {
                users = users.Where(u => u.Role == roleFilter).ToList();
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                bool isActive = statusFilter == "Active";
                users = users.Where(u => u.IsActive == isActive).ToList();
            }

            // Create user management view model
            var viewModel = new UserManagementViewModel
            {
                Users = users.Select(u => new UserWithRolesDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Roles = new List<string> { u.Role },
                    IsActive = u.IsActive,
                }).ToList(),
                AvailableRoles = new List<string> { "Admin", "Lecturer", "Student" },
                SearchTerm = searchTerm,
                RoleFilter = roleFilter,
                StatusFilter = statusFilter
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Statistics(string period = "monthly")
        {
            var courses = await _courseService.GetAllCoursesAsync();
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();

            // Calculate completion rate
            int totalEnrollments = enrollments.Count();
            int completedCourses = enrollments.Count(e => e.Progress == 100);
            decimal completionRate = totalEnrollments > 0
                ? Math.Round((decimal)completedCourses / totalEnrollments * 100, 1)
                : 0;

            // Generate statistics based on period
            string periodName = period == "weekly" ? "Weekly" :
                                period == "yearly" ? "Yearly" : "Monthly";

            var viewModel = new StatisticsViewModel
            {
                Period = period,
                PeriodName = periodName,
                EnrollmentCount = totalEnrollments,
                CompletedCourses = completedCourses,
                CompletionRate = completionRate,
                TopCourses = courses
                    .OrderByDescending(c => enrollments.Count(e => e.CourseId == c.Id))
                    .Take(5)
                    .ToList(),
                RecentEnrollments = enrollments
                    .OrderByDescending(e => e.EnrollmentDate)
                    .Take(10)
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto, List<string> roles)
        {
            if (string.IsNullOrEmpty(userDto.FullName) || string.IsNullOrEmpty(userDto.Email) || string.IsNullOrEmpty(userDto.Password))
            {
                TempData["Error"] = "Username, email, and password are required.";
                return RedirectToAction("UserManagement");
            }

            // Set user role based on selected roles (for simplicity, use the first selected role)
            userDto.Role = roles.FirstOrDefault() ?? "Student";

            var result = await _userService.CreateUserAsync(userDto);
            if (result == null)
            {
                TempData["Success"] = "User created successfully.";
            }
            else
            {
                TempData["Error"] = $"Failed to create user: {result}";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UpdateUserAdminDto userDto)
        {
            if (string.IsNullOrEmpty(userDto.FullName) || string.IsNullOrEmpty(userDto.Email))
            {
                TempData["Error"] = "Full name and email are required.";
                return RedirectToAction("UserManagement");
            }

            var result = await _userService.UpdateUserByAdminAsync(userDto);
            if (result == null)
            {
                TempData["Success"] = "User updated successfully.";
            }
            else
            {
                TempData["Error"] = $"Failed to update user: {result}";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRoles(int id, List<string> roles)
        {
            if (roles == null || !roles.Any())
            {
                TempData["Error"] = "At least one role must be selected.";
                return RedirectToAction("UserManagement");
            }

            // Get the user
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("UserManagement");
            }

            // Update the user's role (for simplicity, just use the first selected role)
            var updateDto = new UpdateUserAdminDto
            {
                Id = id,
                Email = user.Email,
                FullName = user.FullName,
                Role = roles.FirstOrDefault()
            };

            var result = await _userService.UpdateUserByAdminAsync(updateDto);
            if (result == null)
            {
                TempData["Success"] = "User roles updated successfully.";
            }
            else
            {
                TempData["Error"] = $"Failed to update user roles: {result}";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var result = await _userService.DeactivateUserAsync(id);
            if (result)
            {
                TempData["Success"] = "User deactivated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to deactivate user.";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var result = await _userService.ActivateUserAsync(id);
            if (result)
            {
                TempData["Success"] = "User activated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to activate user.";
            }

            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result)
            {
                TempData["Success"] = "User deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete user.";
            }

            return RedirectToAction("UserManagement");
        }

        public IActionResult Reports()
        {
            // This could be implemented to generate various system reports
            return View();
        }

        public async Task<IActionResult> ActivityLog()
        {
            // This could show a complete activity log
            return View();
        }
    }
}