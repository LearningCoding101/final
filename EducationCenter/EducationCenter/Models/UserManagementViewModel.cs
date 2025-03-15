using BLL.DTO.User;
using System;
using System.Collections.Generic;

namespace EducationCenter.Models
{
    public class UserManagementViewModel
    {
        public List<UserWithRolesDto> Users { get; set; }
        public List<string> AvailableRoles { get; set; }
        public string SearchTerm { get; set; }
        public string RoleFilter { get; set; }
        public string StatusFilter { get; set; }
    }

    public class UserWithRolesDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }
    }
}