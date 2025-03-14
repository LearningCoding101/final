using BLL.DTO.Course;
using BLL.DTO.CourseCategory;
using BLL.Interface;
using DAL.Entities;
using DAL.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CourseListDto>> GetAllCoursesAsync()
        {
            var courses = await _unitOfWork.Courses.GetAllWithDetailsAsync();
            return courses.Select(course => new CourseListDto
            {
                Id = course.Id,
                Title = course.Title,
                Type = course.Type,
                Price = course.Price,
                LecturerName = course.Lecturer.FullName,
                CategoryName = course.Category.Name
            });
        }

        public async Task<CourseDto?> GetCourseByIdAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdWithDetailsAsync(id);
            if (course == null) return null;

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Type = course.Type,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CategoryId = course.CategoryId,
                LecturerId = course.LecturerId
            };
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto courseDto)
        {
            var course = new Course
            {
                Title = courseDto.Title,
                Description = courseDto.Description,
                Type = courseDto.Type,
                Price = courseDto.Price,
                StartDate = courseDto.StartDate,
                EndDate = courseDto.EndDate,
                CategoryId = courseDto.CategoryId,
                LecturerId = courseDto.LecturerId
            };

            await _unitOfWork.Courses.AddAsync(course);
            await _unitOfWork.SaveChangesAsync();

            return new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Type = course.Type,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                CategoryId = course.CategoryId,
                LecturerId = course.LecturerId
            };
        }

        public async Task<bool> UpdateCourseAsync(int id, UpdateCourseDto courseDto)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null) return false;

            if (!string.IsNullOrEmpty(courseDto.Title)) course.Title = courseDto.Title;
            if (!string.IsNullOrEmpty(courseDto.Description)) course.Description = courseDto.Description;
            if (!string.IsNullOrEmpty(courseDto.Type)) course.Type = courseDto.Type;
            if (courseDto.Price.HasValue) course.Price = courseDto.Price.Value;
            if (courseDto.StartDate.HasValue) course.StartDate = courseDto.StartDate.Value;
            if (courseDto.EndDate.HasValue) course.EndDate = courseDto.EndDate.Value;
            if (courseDto.CategoryId.HasValue) course.CategoryId = courseDto.CategoryId.Value;
            if (courseDto.LecturerId.HasValue) course.LecturerId = courseDto.LecturerId.Value;

            _unitOfWork.Courses.Update(course);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCourseAsync(int id)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(id);
            if (course == null) return false;

            _unitOfWork.Courses.Delete(course);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseListDto>> GetCoursesByCategoryAsync(int categoryId)
        {
            var courses = await _unitOfWork.Courses.GetCoursesByCategoryAsync(categoryId);
            return courses.Select(course => new CourseListDto
            {
                Id = course.Id,
                Title = course.Title,
                Type = course.Type,
                Price = course.Price,
                LecturerName = course.Lecturer.FullName,
                CategoryName = course.Category.Name
            });
        }

        public async Task<IEnumerable<CourseListDto>> GetCoursesByLecturerAsync(int lecturerId)
        {
            var courses = await _unitOfWork.Courses.GetCoursesByLecturerAsync(lecturerId);
            return courses.Select(course => new CourseListDto
            {
                Id = course.Id,
                Title = course.Title,
                Type = course.Type,
                Price = course.Price,
                LecturerName = course.Lecturer.FullName,
                CategoryName = course.Category.Name
            });
        }

        public async Task<IEnumerable<CourseCategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.CourseCategories.GetAllCategoriesAsync();
            return categories.Select(category => new CourseCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            });
        }
    }
}