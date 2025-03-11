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
    internal class DiscussionRepository : Repository<Discussion>, IDiscussionRepository
    {
        public DiscussionRepository(Context context) : base(context) { }

        public async Task<IEnumerable<Discussion>> GetDiscussionsByCourseIdAsync(int courseId)
        {
            return await _context.Discussions.Where(d => d.CourseId == courseId).ToListAsync();
        }
    }
}
