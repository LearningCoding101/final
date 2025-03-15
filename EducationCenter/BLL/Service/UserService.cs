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
            if (user == null ||
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password) == PasswordVerificationResult.Failed ||
                !user.IsActive)
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
                DepartmentId = registerDto.DepartmentId,
                IsActive = true
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
            return user != null && user.IsActive ? new UserDto
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
            if (user == null || !user.IsActive) return false;

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
            if (user == null ||
                !user.IsActive ||
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordDto.CurrentPassword) == PasswordVerificationResult.Failed)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, passwordDto.NewPassword);
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            return user != null && user.IsActive ? new UserDto
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
            var user = await _unitOfWork.Users.GetByEmailAsync(forgotPasswordDto.Email);
            if (user == null || !user.IsActive) return false;

            // Implementation of password reset logic would go here
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {

            // Implementation of password reset logic would go here
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            return users.Select(d => new UserDto
            {
                Id = d.Id,
                Email = d.Email,
                FullName = d.FullName,
                Role = d.Role,
                ProfilePictureUrl = d.ProfilePictureUrl,
                DepartmentId = d.DepartmentId ?? 0,
                IsActive = d.IsActive
            }).ToList();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user != null)
            {
                _unitOfWork.Users.Delete(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> CreateUserAsync(CreateUserDto userDto)
        {
            // Check if email already exists
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(userDto.Email);
            if (existingUser != null)
            {
                return "Email is already in use";
            }

            // Create new user
            var user = new User
            {
                Email = userDto.Email,
                FullName = userDto.FullName,
                Role = userDto.Role,
                IsActive = true,
            };

            // Hash the password
            user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return null; // Return null for success
        }

        public async Task<string> UpdateUserByAdminAsync(UpdateUserAdminDto updateDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(updateDto.Id);
            if (user == null)
            {
                return "User not found";
            }

            // Update user properties
            user.Email = updateDto.Email;
            user.FullName = updateDto.FullName;
            user.Role = updateDto.Role;

            // If password is provided, update it
            if (!string.IsNullOrEmpty(updateDto.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, updateDto.Password);
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return null; // Return null for success
        }

        public async Task<bool> DeactivateUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = false;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateUserAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            user.IsActive = true;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}