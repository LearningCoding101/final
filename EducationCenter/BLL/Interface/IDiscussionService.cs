using BLL.DTO.Discussion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface IDiscussionService
    {
        Task<IEnumerable<DiscussionDto>> GetAllDiscussionsAsync();
        Task<IEnumerable<DiscussionDto>> GetDiscussionsByCourseAsync(int courseId);
        Task<IEnumerable<DiscussionDto>> GetDiscussionsByUserAsync(int userId);
        Task<DiscussionDto?> GetDiscussionByIdAsync(int id);
        Task<DiscussionDto> CreateDiscussionAsync(CreateDiscussionDto discussionDto);
        Task<bool> UpdateDiscussionAsync(int id, UpdateDiscussionDto discussionDto);
        Task<bool> DeleteDiscussionAsync(int id);
    }
}
