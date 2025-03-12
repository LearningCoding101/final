using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.CourseMaterial
{
    public class UpdateCourseMaterialDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }

        public string? FileUrl { get; set; }
    }
}
