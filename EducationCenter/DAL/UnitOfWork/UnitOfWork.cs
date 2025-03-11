using DAL.Data;
using DAL.Interface;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;

        public IUserRepository Users { get; }
        public IDepartmentRepository Departments { get; }
        public ICourseRepository Courses { get; }
        public IEnrollmentRepository Enrollments { get; }
        public ICourseMaterialRepository CourseMaterials { get; }
        public ICourseScheduleRepository CourseSchedules { get; }
        public ICourseCategoryRepository CourseCategories { get; }
        public INewsRepository News { get; }
        public IDiscussionRepository Discussions { get; }
        public IMessageRepository Messages { get; }

        public UnitOfWork(Context context)
        {
            _context = context;
            Users = new UserRepository(context);
            Departments = new DepartmentRepository(context);
            Courses = new CourseRepository(context);
            Enrollments = new EnrollmentRepository(context);
            CourseMaterials = new CourseMaterialRepository(context);
            CourseSchedules = new CourseScheduleRepository(context);
            CourseCategories = new CourseCategoryRepository(context);
            News = new NewsRepository(context);
            Discussions = new DiscussionRepository(context);
            Messages = new MessageRepository(context);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
