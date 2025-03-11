using DAL.Data;
using DAL.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    internal class CourseScheduleRepository : Repository<CourseSchedule>, ICourseScheduleRepository
    {
        public CourseScheduleRepository(Context context) : base(context) { }

        public async Task<IEnumerable<CourseSchedule>> GetSchedulesByCourseIdAsync(int courseId)
        {
            return await _context.CourseSchedules.Where(cs => cs.CourseId == courseId).ToListAsync();
        }
    }
}
