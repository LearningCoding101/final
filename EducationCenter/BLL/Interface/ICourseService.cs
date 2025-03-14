using BLL.DTO.Course;
using BLL.DTO.CourseCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListDto>> GetAllCoursesAsync();
        Task<CourseDto?> GetCourseByIdAsync(int id);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto courseDto);
        Task<bool> UpdateCourseAsync(int id, UpdateCourseDto courseDto);
        Task<bool> DeleteCourseAsync(int id);
        Task<IEnumerable<CourseListDto>> GetCoursesByCategoryAsync(int categoryId);
        Task<IEnumerable<CourseListDto>> GetCoursesByLecturerAsync(int lecturerId);
        Task<IEnumerable<CourseCategoryDto>> GetAllCategoriesAsync();
    }

}
