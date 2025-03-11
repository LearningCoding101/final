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
    internal class CourseMaterialRepository : Repository<CourseMaterial>, ICourseMaterialRepository
    {
        public CourseMaterialRepository(Context context) : base(context) { }

        public async Task<IEnumerable<CourseMaterial>> GetMaterialsByCourseIdAsync(int courseId)
        {
            return await _context.CourseMaterials.Where(m => m.CourseId == courseId).ToListAsync();
        }
    }
}
