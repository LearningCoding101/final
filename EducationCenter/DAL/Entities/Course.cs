using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Online/Offline
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }
        public CourseCategory Category { get; set; }

        public int LecturerId { get; set; }
        public User Lecturer { get; set; }

        // Relationships
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<CourseMaterial> Materials { get; set; } = new List<CourseMaterial>();
        public ICollection<CourseSchedule> Schedules { get; set; } = new List<CourseSchedule>();
        public ICollection<Discussion> Discussions { get; set; } = new List<Discussion>();
    }


}