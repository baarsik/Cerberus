using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models;
using DataContext;
using DataContext.Models;
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

            model.Items = _db.WebNovels
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (model.Page - 1))
                .Include(c => c.Chapters)
                .Select(GetWebNovelInfo)
                .ToList();
            
            return model;
        }
        
        public async Task<WebNovelDetailsViewModel> GetWebNovelDetailsViewModelAsync(string webNovelUrl)
        {
            var webNovel = await _db.WebNovels
                .Include(c => c.Chapters)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            var model = new WebNovelDetailsViewModel
            {
                WebNovelInfo = GetWebNovelInfo(webNovel),
                IsValid = webNovel != null
            };
            
            return model;
        }

        private WebNovelInfo GetWebNovelInfo(WebNovel webNovel)
        {
            if (webNovel?.Chapters == null)
            {
                return null;
            }

            return new WebNovelInfo
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
            };
        }
    }
}