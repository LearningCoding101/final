using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Enrollment
{
    public class CreateEnrollmentDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Pending"; // Default to Pending
    }
}
