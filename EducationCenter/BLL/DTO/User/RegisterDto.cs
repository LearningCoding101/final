using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.User
{
    public class RegisterDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        public string Role { get; set; } = "Student"; // Default role is "Student"

        public string ProfilePictureUrl { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }

}
