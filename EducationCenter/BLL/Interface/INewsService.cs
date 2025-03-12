using BLL.DTO.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<NewsDto?> GetNewsByIdAsync(int id);
        Task<NewsDto> CreateNewsAsync(CreateNewsDto newsDto);
        Task<bool> UpdateNewsAsync(int id, UpdateNewsDto newsDto);
        Task<bool> DeleteNewsAsync(int id);
    }
}
