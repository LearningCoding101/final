using BLL.DTO.News;
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
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            var newsList = await _unitOfWork.News.GetAllAsync();

            return newsList.Select(n => new NewsDto
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublishDate = n.PublishDate,
                Type = n.Type,
                ThumbnailUrl = n.ThumbnailUrl,
                EventDate = n.EventDate,
                EventLocation = n.EventLocation
            });
        }

        public async Task<NewsDto?> GetNewsByIdAsync(int id)
        {
            var news = await _unitOfWork.News.GetByIdAsync(id);
            if (news == null) return null;

            return new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                PublishDate = news.PublishDate,
                Type = news.Type,
                ThumbnailUrl = news.ThumbnailUrl,
                EventDate = news.EventDate,
                EventLocation = news.EventLocation
            };
        }

        public async Task<NewsDto> CreateNewsAsync(CreateNewsDto newsDto)
        {
            var news = new News
            {
                Title = newsDto.Title,
                Content = newsDto.Content,
                PublishDate = DateTime.UtcNow,
                Type = newsDto.Type,
                ThumbnailUrl = newsDto.ThumbnailUrl,
                EventDate = newsDto.EventDate,
                EventLocation = newsDto.EventLocation
            };

            await _unitOfWork.News.AddAsync(news);
            await _unitOfWork.SaveChangesAsync();

            return new NewsDto
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                PublishDate = news.PublishDate,
                Type = news.Type,
                ThumbnailUrl = news.ThumbnailUrl,
                EventDate = news.EventDate,
                EventLocation = news.EventLocation
            };
        }

        public async Task<bool> UpdateNewsAsync(int id, UpdateNewsDto newsDto)
        {
            var news = await _unitOfWork.News.GetByIdAsync(id);
            if (news == null) return false;

            if (!string.IsNullOrEmpty(newsDto.Title)) news.Title = newsDto.Title;
            if (!string.IsNullOrEmpty(newsDto.Content)) news.Content = newsDto.Content;
            if (!string.IsNullOrEmpty(newsDto.Type)) news.Type = newsDto.Type;
            if (!string.IsNullOrEmpty(newsDto.ThumbnailUrl)) news.ThumbnailUrl = newsDto.ThumbnailUrl;
            if (newsDto.EventDate.HasValue) news.EventDate = newsDto.EventDate;
            if (!string.IsNullOrEmpty(newsDto.EventLocation)) news.EventLocation = newsDto.EventLocation;

            _unitOfWork.News.Update(news);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _unitOfWork.News.GetByIdAsync(id);
            if (news == null) return false;

            _unitOfWork.News.Delete(news);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }

}
