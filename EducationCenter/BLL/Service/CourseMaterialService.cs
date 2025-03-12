using BLL.DTO.CourseMaterial;
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
    public class CourseMaterialService : ICourseMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseMaterialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CourseMaterialDto> AddMaterialAsync(CreateCourseMaterialDto materialDto)
        {
            var courseExists = await _unitOfWork.CourseMaterials.GetByIdAsync(materialDto.CourseId);
            if (courseExists == null)
            {
                Console.WriteLine("Course doesnt exist");
                return null;
            }
            var material = new CourseMaterial
            {
                CourseId = materialDto.CourseId,
                Title = materialDto.Title,
                FileUrl = materialDto.FileUrl,
                UploadDate = DateTime.UtcNow
            };

            await _unitOfWork.CourseMaterials.AddAsync(material);
            await _unitOfWork.SaveChangesAsync();

            return new CourseMaterialDto
            {
                Id = material.Id,
                CourseId = material.CourseId,
                CourseTitle = (await _unitOfWork.Courses.GetByIdAsync(material.CourseId))?.Title ?? "Unknown",
                Title = material.Title,
                FileUrl = material.FileUrl,
                UploadDate = material.UploadDate
            };


        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            var material = await _unitOfWork.CourseMaterials.GetByIdAsync(id);
            if (material == null) return false;

            _unitOfWork.CourseMaterials.Delete(material);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CourseMaterialDto>> GetAllMaterialsAsync()
        {
            var materials = await _unitOfWork.CourseMaterials.GetAllAsync();

            return materials.Select(material => new CourseMaterialDto
            {
                Id = material.Id,
                CourseId = material.CourseId,
                CourseTitle = material.Course.Title,
                Title = material.Title,
                FileUrl = material.FileUrl,
                UploadDate = material.UploadDate
            });
        }
        public async Task<CourseMaterialDto?> GetMaterialByIdAsync(int id)
        {
            var material = await _unitOfWork.CourseMaterials.GetByIdAsync(id);
            if (material == null) return null;

            return new CourseMaterialDto
            {
                Id = material.Id,
                CourseId = material.CourseId,
                CourseTitle = material.Course.Title,
                Title = material.Title,
                FileUrl = material.FileUrl,
                UploadDate = material.UploadDate
            };
        }

        public async Task<IEnumerable<CourseMaterialDto>> GetMaterialsByCourseAsync(int courseId)
        {
            var materials = await _unitOfWork.CourseMaterials.GetMaterialsByCourseIdAsync(courseId);

            return materials.Select(material => new CourseMaterialDto
            {
                Id = material.Id,
                CourseId = material.CourseId,
                CourseTitle = material.Course.Title,
                Title = material.Title,
                FileUrl = material.FileUrl,
                UploadDate = material.UploadDate
            });
        }

        public async Task<bool> UpdateMaterialAsync(int id, UpdateCourseMaterialDto materialDto)
        {
            var material = await _unitOfWork.CourseMaterials.GetByIdAsync(id);
            
            if (material == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(materialDto.Title))
            {
                material.Title = materialDto.Title;
            }
            if (!string.IsNullOrEmpty(materialDto.FileUrl))
            {
                material.FileUrl = materialDto.FileUrl;
            }

            _unitOfWork.CourseMaterials.Update(material);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
