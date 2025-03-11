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
            return await _context.Set<Course>()
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetCoursesByLecturerAsync(int lecturerId)
        {
            return await _context.Set<Course>()
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .Where(c => c.LecturerId == lecturerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Course>()
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .ToListAsync();
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Course>()
                .Include(c => c.Lecturer)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
