using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; } // Admin, Lecturer, Student
        public string? ProfilePictureUrl { get; set; } = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtbAKcCLLFn6XZUsNl16-pGGH1Aj6Z01s9OQ&s";

        public bool IsActive { get; set; } = true;
        // Foreign key to Department
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // Relationships
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    }

}
