﻿using DAL.Data;
using DAL.Entities;
using DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    internal class CourseCategoryRepository : Repository<CourseCategory>, ICourseCategoryRepository
    {
        public CourseCategoryRepository(Context context) : base(context) { }
    }
}
