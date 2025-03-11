using BLL.DTO.User;
using BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class UserService : IUserService
    {
        public Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePasswordAsync(ChangePasswordDto passwordDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto?> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
