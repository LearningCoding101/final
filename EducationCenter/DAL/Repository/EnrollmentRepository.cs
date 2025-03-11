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
    internal class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
    {
        public EnrollmentRepository(Context context) : base(context) { }

        public async Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(int userId)
        {
            return await _context.Enrollments.Where(e => e.UserId == userId).ToListAsync();
        }
    }
}
