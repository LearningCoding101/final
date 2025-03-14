using BLL.DTO.Enrollment;
using BLL.Interface;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        const int MIN_PROGRESS = 0;
        const int MAX_PROGRESS = 100;
        public EnrollmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteEnrollmentAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null) return false;

            _unitOfWork.Enrollments.Delete(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<EnrollmentDto> EnrollStudentAsync(CreateEnrollmentDto enrollmentDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(enrollmentDto.UserId);
            var course = await _unitOfWork.Courses.GetByIdAsync(enrollmentDto.CourseId);

            if (user == null || course == null)
            {
                throw new InvalidOperationException("User or Course not found.");
            }

            var enrollment = new Enrollment
            {
                UserId = enrollmentDto.UserId,
                CourseId = enrollmentDto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                PaymentStatus = enrollmentDto.PaymentStatus,
                Progress = 0 // Default to 0% progress
            };

            await _unitOfWork.Enrollments.AddAsync(enrollment);
            await _unitOfWork.SaveChangesAsync();

            return new EnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                StudentName = (await _unitOfWork.Users.GetByIdAsync(enrollment.UserId))?.FullName ?? "Unknown",
                CourseId = enrollment.CourseId,
                CourseTitle = (await _unitOfWork.Courses.GetByIdAsync(enrollment.CourseId))?.Title ?? "Unknown",
                EnrollmentDate = enrollment.EnrollmentDate,
                PaymentStatus = enrollment.PaymentStatus,
                Progress = enrollment.Progress
            };
        }

        public async Task<IEnumerable<EnrollmentDto>> GetAllEnrollmentsAsync()
        {
            var enrollments = await _unitOfWork.Enrollments.GetAllAsync();

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                UserId = e.UserId,
                StudentName = e.User.FullName,
                CourseId = e.CourseId,
                CourseTitle = e.Course.Title,
                EnrollmentDate = e.EnrollmentDate,
                PaymentStatus = e.PaymentStatus,
                Progress = e.Progress
            });
        }

        public async Task<EnrollmentDto?> GetEnrollmentByIdAsync(int id)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null) return null;

            return new EnrollmentDto
            {
                Id = enrollment.Id,
                UserId = enrollment.UserId,
                StudentName = enrollment.User.FullName,
                CourseId = enrollment.CourseId,
                CourseTitle = enrollment.Course.Title,
                EnrollmentDate = enrollment.EnrollmentDate,
                PaymentStatus = enrollment.PaymentStatus,
                Progress = enrollment.Progress
            };
        }

        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByCourseAsync(int courseId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetByCourseAsync(courseId);

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                UserId = e.UserId,
                StudentName = e.User.FullName,
                CourseId = e.CourseId,
                CourseTitle = e.Course.Title,
                EnrollmentDate = e.EnrollmentDate,
                PaymentStatus = e.PaymentStatus,
                Progress = e.Progress
            });
        }
        public async Task<IEnumerable<EnrollmentDto>> GetEnrollmentsByUserAsync(int userId)
        {
            var enrollments = await _unitOfWork.Enrollments.GetEnrollmentsByUserIdAsync(userId);

            return enrollments.Select(e => new EnrollmentDto
            {
                Id = e.Id,
                UserId = e.UserId,
                StudentName = e.User?.FullName ?? "Unknown",
                CourseId = e.CourseId,
                CourseTitle = e.Course?.Title ?? "Unknown",
                EnrollmentDate = e.EnrollmentDate,
                PaymentStatus = e.PaymentStatus,
                Progress = e.Progress
            });
        }

        public async Task<bool> UpdateEnrollmentAsync(int id, UpdateEnrollmentDto enrollmentDto)
        {
            var enrollment = await _unitOfWork.Enrollments.GetByIdAsync(id);
            if (enrollment == null) return false;

            if (!string.IsNullOrEmpty(enrollmentDto.PaymentStatus))
                enrollment.PaymentStatus = enrollmentDto.PaymentStatus;

            if (enrollmentDto.Progress.HasValue)
                enrollment.Progress = Math.Clamp(enrollmentDto.Progress.Value, MIN_PROGRESS, MAX_PROGRESS);

            _unitOfWork.Enrollments.Update(enrollment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
