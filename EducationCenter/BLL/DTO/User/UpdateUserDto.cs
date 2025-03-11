using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.User
{
    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public int? DepartmentId { get; set; }
    }
}
