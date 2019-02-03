using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models;
using Cerberus.Models.Helpers;
using Cerberus.Models.ViewModels;
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
                .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            var model = new WebNovelDetailsViewModel
            {
                WebNovelInfo = GetWebNovelInfo(webNovel),
                IsValid = webNovel != null
            };
            
            return model;
        }
        
        public async Task<AddChapterViewModel> GetWebNovelAddChapterViewModelAsync(Guid webNovelId, AddChapterViewModel model = null)
        {
            if (model == null)
            {
                model = new AddChapterViewModel
                {
                    WebNovelId = webNovelId
                };
            }

            model.WebNovel = await GetWebNovelByIdAsync(model.WebNovelId);
            model.Languages = await GetLanguagesAsync();

            var lastChapter = model.WebNovel.Chapters.OrderByDescending(c => c.CreationDate).FirstOrDefault();
            if (lastChapter == null)
            {
                model.Volume = model.Volume == 0 ? 1 : model.Volume;
                model.Number = 1;
            }
            else
            {
                model.Volume = model.Volume == 0 ? lastChapter.Volume : model.Volume;
                model.Number = lastChapter.Number + 1;
            }
            
            return model;
        }

        public async Task<WebNovel> GetWebNovelByIdAsync(Guid id)
        {
            return await _db.WebNovels
                .Include(c => c.Chapters)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _db.Languages.ToListAsync();
        }

        public async Task<WebNovelAddWebNovelResult> AddWebNovelAsync(ApplicationUser user, AddWebNovelViewModel model)
        {
            var url = model.UrlName.ToLower();
            if (await _db.WebNovels.AnyAsync(c => c.UrlName.ToLower() == url))
                return WebNovelAddWebNovelResult.WebNovelUrlExists;
            
            var webNovel = new WebNovel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                OriginalName = model.OriginalName,
                UrlName = url,
                Description = model.Description,
                UsesVolumes = model.UsesVolumes,
                Author = model.Author,
                CreationDate = DateTime.Now
            };
            _db.WebNovels.Add(webNovel);
            await _db.SaveChangesAsync();
            return WebNovelAddWebNovelResult.Success;
        }
        
        public async Task<WebNovelAddChapterResult> AddChapterAsync(ApplicationUser uploader, AddChapterViewModel model)
        {
            var webNovel = await _db.WebNovels
                .Include(c => c.Chapters)
                .FirstOrDefaultAsync(c => c.Id == model.WebNovelId);
            
            if (webNovel == null)
                return WebNovelAddChapterResult.WebNovelNotExists;

            if (!webNovel.UsesVolumes)
            {
                model.Volume = 1;
            }

            if (webNovel.Chapters.Any(c => c.Volume == model.Volume && c.Number == model.Number))
                return WebNovelAddChapterResult.NumberExists;

            var languageId = await _db.Languages.FirstOrDefaultAsync(c => c.Id == model.LanguageId);
            
            if (languageId == null)
                return WebNovelAddChapterResult.LanguageNotExists;

            var previousChapter = webNovel.Chapters.OrderBy(c => c.CreationDate).LastOrDefault();
            var chapter = new WebNovelChapter
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.Now,
                FreeToAccessDate = model.FreeToAccessDate,
                Volume = model.Volume,
                Number = model.Number,
                Title = model.Title,
                PreviousChapter = previousChapter,
                Text = model.Text.SanitizeHTML(),
                Uploader = uploader,
                WebNovel = webNovel,
                Language = languageId
            };

            _db.WebNovelChapters.Add(chapter);
            
            if (previousChapter != null)
            {
                previousChapter.NextChapter = chapter;
                _db.Update(previousChapter);
            }

            await _db.SaveChangesAsync();
            return WebNovelAddChapterResult.Success;
        }
        
        public async Task<WebNovelChapter> GetChapterAsync(string webNovelUrl, string chapterNumber)
        {
            var webNovel = await _db.WebNovels
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            if (webNovel == null || !int.TryParse(chapterNumber, out var number))
                return null;

            var chapter = _db.WebNovelChapters
                .Include(c => c.PreviousChapter)
                .Include(c => c.NextChapter)
                .SingleOrDefault(c => c.WebNovel == webNovel && c.Number == number);
            
            if (chapter == null)
                return null;
            
            chapter.WebNovel = webNovel;
            return chapter;
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

    public enum WebNovelAddWebNovelResult
    {
        UnknownFailure,
        WebNovelUrlExists,
        Success
    }
    
    public enum WebNovelAddChapterResult
    {
        UnknownFailure,
        WebNovelNotExists,
        NumberExists,
        LanguageNotExists,
        Success
    }
}