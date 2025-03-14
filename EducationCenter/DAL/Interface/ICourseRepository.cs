﻿using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByCategoryAsync(int categoryId);
        Task<IEnumerable<Course>> GetCoursesByLecturerAsync(int lecturerId);
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task<Course?> GetByIdWithDetailsAsync(int id);
    }
}
