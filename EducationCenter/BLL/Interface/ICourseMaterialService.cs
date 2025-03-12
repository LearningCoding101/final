using BLL.DTO.CourseMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICourseMaterialService
    {
        Task<IEnumerable<CourseMaterialDto>> GetAllMaterialsAsync();
        Task<CourseMaterialDto?> GetMaterialByIdAsync(int id);
        Task<IEnumerable<CourseMaterialDto>> GetMaterialsByCourseAsync(int courseId);
        Task<CourseMaterialDto> AddMaterialAsync(CreateCourseMaterialDto materialDto);
        Task<bool> UpdateMaterialAsync(int id, UpdateCourseMaterialDto materialDto);
        Task<bool> DeleteMaterialAsync(int id);
    }
}
