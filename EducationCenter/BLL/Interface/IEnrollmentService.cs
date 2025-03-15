using BLL.DTO.Enrollment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IEnrollmentService
    {
        Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync();
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByUserAsync(int userId);
        Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByCourseAsync(int courseId);
        Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id);
        Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto enrollmentDto);
        Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto enrollmentDto);
        Task<bool> DeleteEnrollmentAsync(int id);

        Task<bool> UpdateProgressAsync(int enrollmentId, int progress);
    }
}
