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
    internal class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(Context context) : base(context) { }

        public async Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId)
        {
            return await _context.Courses.Where(c => c.CategoryId == categoryId).ToListAsync();
        }
    }
}
