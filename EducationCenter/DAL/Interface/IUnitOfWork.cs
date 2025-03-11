using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IDepartmentRepository Departments { get; }
        ICourseRepository Courses { get; }
        IEnrollmentRepository Enrollments { get; }
        ICourseMaterialRepository CourseMaterials { get; }
        ICourseScheduleRepository CourseSchedules { get; }
        ICourseCategoryRepository CourseCategories { get; }
        INewsRepository News { get; }
        IDiscussionRepository Discussions { get; }
        IMessageRepository Messages { get; }

        Task<int> SaveChangesAsync();
    }
}
