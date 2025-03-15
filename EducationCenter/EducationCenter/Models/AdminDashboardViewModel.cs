using BLL.DTO.User;
using System;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class AdminDashboardViewModel

    {
        public int TotalUsers { get; set; }
        public int StudentCount { get; set; }
        public int LecturerCount { get; set; }
        public int AdminCount { get; set; }
        public int ActiveCourses { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveEnrollments { get; set; }
        public List<string> RevenueMonths { get; set; }
        public List<decimal> RevenueValues { get; set; }
        public List<RecentActivityDto> RecentActivities { get; set; }
    }

    public class RecentActivityDto
    {
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Detail { get; set; }
        public DateTime Timestamp { get; set; }
    }
}