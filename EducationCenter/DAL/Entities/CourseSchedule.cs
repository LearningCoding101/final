using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CourseSchedule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime SessionDate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public string Location { get; set; }
    }

}
