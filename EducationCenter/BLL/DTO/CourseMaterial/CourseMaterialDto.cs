using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO.CourseMaterial
{
    public class CourseMaterialDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public string CourseTitle { get; set; }

        public string Title { get; set; }
        public string FileUrl { get; set; }

        public DateTime UploadDate { get; set; }
    }
}
