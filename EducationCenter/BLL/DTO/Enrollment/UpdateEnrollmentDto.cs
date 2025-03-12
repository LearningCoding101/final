using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.Enrollment
{
    public class UpdateEnrollmentDto
    {
        public string? PaymentStatus { get; set; } // Paid, Pending, Canceled
        public int? Progress { get; set; } // Update progress (0 - 100)
    }
}
