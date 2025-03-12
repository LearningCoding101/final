using BLL.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IUserService
    {
        Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserByEmailAsync(string email);

        Task<IEnumerable<UserDto>> GetAllUserAsync();
        Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto);
        Task<bool> ChangePasswordAsync(ChangePasswordDto passwordDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
