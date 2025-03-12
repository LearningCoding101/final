using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IDiscussionRepository : IRepository<Discussion>
    {
        Task<IEnumerable<Discussion>> GetDiscussionsByUserIdAsync(int userId);

        Task<IEnumerable<Discussion>> GetDiscussionsByCourseIdAsync(int courseId);
    }

}
