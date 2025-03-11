using DAL.Data;
using DAL.Entities;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    internal class NewsRepository : Repository<News>, INewsRepository
    {
        public NewsRepository(Context context) : base(context) { }

        public async Task<IEnumerable<News>> GetNewsByTypeAsync(string type)
        {
            return await _context.News.Where(n => n.Type == type).ToListAsync();
        }
    }
}
