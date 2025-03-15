using BLL.DTO.Discussion;
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
    public class DiscussionService : IDiscussionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DiscussionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DiscussionDto> CreateDiscussionAsync(CreateDiscussionDto discussionDto)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(discussionDto.UserId);
            var course = await _unitOfWork.Courses.GetByIdAsync(discussionDto.CourseId);

            if (user == null || course == null)
            {
                throw new InvalidOperationException("User or Course not found.");
            }

            var discussion = new Discussion
            {
                CourseId = discussionDto.CourseId,
                UserId = discussionDto.UserId,
                Title = discussionDto.Title ?? "Untitled Discussion",
                Content = discussionDto.Content ?? "No content provided",
                PostDate = DateTime.UtcNow
            };

            await _unitOfWork.Discussions.AddAsync(discussion);
            await _unitOfWork.SaveChangesAsync();

            return new DiscussionDto
            {
                Id = discussion.Id,
                CourseId = discussion.CourseId,
                CourseTitle = (await _unitOfWork.Courses.GetByIdAsync(discussion.CourseId))?.Title ?? "Unknown Course",
                UserId = discussion.UserId,
                UserName = (await _unitOfWork.Users.GetByIdAsync(discussion.UserId))?.FullName ?? "Unknown User",
                Title = discussion.Title ?? "Untitled Discussion",
                Content = discussion.Content ?? "No content provided",
                PostDate = discussion.PostDate
            };
        }

        public async Task<bool> DeleteDiscussionAsync(int id)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);
            if (discussion == null) return false;

            _unitOfWork.Discussions.Delete(discussion);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<DiscussionDto>> GetAllDiscussionsAsync()
        {
            var discussions = await _unitOfWork.Discussions.GetAllAsync();

            return discussions.Select(d => new DiscussionDto
            {
                Id = d.Id,
                CourseId = d.CourseId,
                CourseTitle = d.Course?.Title ?? "Unknown Course",
                UserId = d.UserId,
                UserName = d.User?.FullName ?? "Unknown User",
                Title = d.Title ?? "Untitled Discussion",
                Content = d.Content ?? "No content provided",
                PostDate = d.PostDate
            });
        }

        public async Task<DiscussionDto?> GetDiscussionByIdAsync(int id)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);
            if (discussion == null) return null;

            return new DiscussionDto
            {
                Id = discussion.Id,
                CourseId = discussion.CourseId,
                CourseTitle = discussion.Course?.Title ?? "Unknown Course",
                UserId = discussion.UserId,
                UserName = discussion.User?.FullName ?? "Unknown User",
                Title = discussion.Title ?? "Untitled Discussion",
                Content = discussion.Content ?? "No content provided",
                PostDate = discussion.PostDate
            };
        }


        public async Task<IEnumerable<DiscussionDto>> GetDiscussionsByCourseAsync(int courseId)
        {
            var discussions = await _unitOfWork.Discussions.GetDiscussionsByCourseIdAsync(courseId);

            return discussions.Select(d => new DiscussionDto
            {
                Id = d.Id,
                CourseId = d.CourseId,
                CourseTitle = d.Course?.Title ?? "Unknown Course",
                UserId = d.UserId,
                UserName = d.User?.FullName ?? "Unknown User",
                Title = d.Title ?? "Untitled Discussion",
                Content = d.Content ?? "No content provided",
                PostDate = d.PostDate
            });
        }


        public async Task<IEnumerable<DiscussionDto>> GetDiscussionsByUserAsync(int userId)
        {
            var discussions = await _unitOfWork.Discussions.GetDiscussionsByUserIdAsync(userId);

            return discussions.Select(d => new DiscussionDto
            {
                Id = d.Id,
                CourseId = d.CourseId,
                CourseTitle = d.Course?.Title ?? "Unknown Course",
                UserId = d.UserId,
                UserName = d.User?.FullName ?? "Unknown User",
                Title = d.Title ?? "Untitled Discussion",
                Content = d.Content ?? "No content provided",
                PostDate = d.PostDate
            });
        }


        public async Task<bool> UpdateDiscussionAsync(int id, UpdateDiscussionDto discussionDto)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(id);
            if (discussion == null) return false;

            if (!string.IsNullOrEmpty(discussionDto.Title))
                discussion.Title = discussionDto.Title;
            // If title is null or empty, keep existing title

            if (!string.IsNullOrEmpty(discussionDto.Content))
                discussion.Content = discussionDto.Content;
            // If content is null or empty, keep existing content

            _unitOfWork.Discussions.Update(discussion);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}