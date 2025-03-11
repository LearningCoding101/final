using BLL.DTO.User;
using BLL.Interface;
using DAL.Entities;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;

namespace EducationCenter.BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IJwtService jwtService, IPasswordHasher<User> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDto?> AuthenticateAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed)
                return null;

            var token = _jwtService.GenerateToken(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl,
                DepartmentId = user.DepartmentId ?? 0
            });

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(2),
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role,
                    ProfilePictureUrl = user.ProfilePictureUrl,
                    DepartmentId = user.DepartmentId ?? 0
                }
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await _unitOfWork.Users.GetByEmailAsync(registerDto.Email) != null)
                throw new Exception("Email already exists.");

            var user = new User
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PasswordHash = _passwordHasher.HashPassword(null, registerDto.Password),
                Role = registerDto.Role ?? "Student",
                ProfilePictureUrl = registerDto.ProfilePictureUrl,
                DepartmentId = registerDto.DepartmentId
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl,
                DepartmentId = user.DepartmentId ?? 0
            };
        }

        
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            return user != null ? new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl,
                DepartmentId = user.DepartmentId ?? 0
            } : null;
        }

        public async Task<bool> UpdateUserAsync(int id, UpdateUserDto updateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return false;

            user.FullName = updateDto.FullName;
            user.ProfilePictureUrl = updateDto.ProfilePictureUrl;
            if (updateDto.DepartmentId.HasValue)
                user.DepartmentId = updateDto.DepartmentId.Value;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

       
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto passwordDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(passwordDto.UserId);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordDto.CurrentPassword) == PasswordVerificationResult.Failed)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, passwordDto.NewPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            return user != null ? new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                ProfilePictureUrl = user.ProfilePictureUrl,
                DepartmentId = user.DepartmentId ?? 0
            } : null;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            return true;
        }
    }
}