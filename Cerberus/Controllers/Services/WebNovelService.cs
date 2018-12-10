using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models;
using DataContext;
using Microsoft.EntityFrameworkCore;

namespace Cerberus.Controllers.Services
{
    public sealed class WebNovelService
    {
        private readonly ApplicationContext _db;

        public WebNovelService(ApplicationContext context)
        {
            _db = context;
        }

        public async Task<WebNovelIndexViewModel> GetWebNovelIndexViewModelAsync(int page)
        {
            var totalPages = (int) Math.Ceiling(await _db.WebNovels.CountAsync() / (double) Constants.WebNovel.ItemsPerIndexPage);
            if (totalPages == 0)
            {
                totalPages = 1;
            }

            var model = new WebNovelIndexViewModel
            {
                Page = page < 1
                    ? 1
                    : page > totalPages
                        ? totalPages
                        : page,
                TotalPages = totalPages,
            };

            model.Items = await _db.WebNovels
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (model.Page - 1))
                .Include(c => c.Chapters)
                .Select(webNovel => new WebNovelInfo
                {
                    WebNovel = webNovel,
                    TotalChapters = webNovel.Chapters.Count,
                    LastChapter = webNovel.Chapters
                        .OrderBy(c => c.CreationDate)
                        .LastOrDefault(),
                    TotalVolumes = webNovel.Chapters
                        .Select(c => c.Volume)
                        .OrderByDescending(c => c)
                        .FirstOrDefault()
                })
                .ToListAsync();
            
            return model;
        }
    }
}