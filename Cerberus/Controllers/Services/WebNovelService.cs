using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Models;
using Cerberus.Models.Extensions;
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
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Select(GetWebNovelInfo)
                .OrderByDescending(c => c.LastUpdateDate)
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (model.Page - 1))
                .ToList();
            
            return model;
        }
        
        public async Task<WebNovelDetailsViewModel> GetWebNovelDetailsViewModelAsync(string webNovelUrl)
        {
            var webNovel = await _db.WebNovels
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
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

            var lastChapter = model.WebNovel.GetLastChapter();
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
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
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
                Name = model.Name.RemoveHTML(),
                OriginalName = model.OriginalName.RemoveHTML(),
                UrlName = url.RemoveHTML(),
                Description = model.Description.SanitizeHTML(),
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
                    .ThenInclude(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == model.WebNovelId);
            
            if (webNovel == null)
                return WebNovelAddChapterResult.WebNovelNotExists;

            if (!webNovel.UsesVolumes)
            {
                model.Volume = 1;
            }

            var language = await _db.Languages.FirstOrDefaultAsync(c => c.Id == model.LanguageId);
            
            if (language == null)
                return WebNovelAddChapterResult.LanguageNotExists;

            var chapter = webNovel.Chapters.FirstOrDefault(c => c.Volume == model.Volume && c.Number == model.Number);
            
            if (chapter != null && webNovel.Chapters.Any(c => c.Translations.Any(x => x.Language == language)))
            {
                return WebNovelAddChapterResult.TranslatedChapterNumberExists;
            }

            if (chapter == null)
            {
                var previousChapter =
                    webNovel.Chapters
                        .Where(c => c.Volume == model.Volume && c.Number < model.Number)
                        .OrderByDescending(c => c.Number)
                        .FirstOrDefault()
                    ?? webNovel.Chapters // In case there is no previous chapter in this volume
                        .Where(c => c.Volume < model.Volume)
                        .OrderByDescending(c => c.Volume)
                        .ThenByDescending(c => c.Number)
                        .FirstOrDefault();

                var nextChapter = previousChapter == null
                    ? webNovel.Chapters // This is now the first chapter, select previous first one as NextChapter
                        .Where(c => c.Volume >= model.Volume)
                        .OrderBy(c => c.Volume)
                        .ThenBy(c => c.Number)
                        .FirstOrDefault()
                    : previousChapter.NextChapter;

                chapter = new WebNovelChapter
                {
                    Id = Guid.NewGuid(),
                    Volume = model.Volume,
                    Number = model.Number,
                    WebNovel = webNovel
                };
                _db.WebNovelChapters.Add(chapter);
                await _db.SaveChangesAsync();

                if (previousChapter != null)
                {
                    previousChapter.NextChapter = chapter;
                    _db.Update(previousChapter);
                }

                if (nextChapter != null)
                {
                    nextChapter.PreviousChapter = chapter;
                    _db.Update(nextChapter);
                }

                chapter.PreviousChapter = previousChapter;
                chapter.NextChapter = nextChapter;
                _db.Update(chapter);
                await _db.SaveChangesAsync();
            }

            var chapterContent = new WebNovelChapterContent
            {
                Id = Guid.NewGuid(),
                Title = model.Title.RemoveHTML(),
                Text = model.Text.SanitizeHTML(),
                CreationDate = DateTime.Now,
                FreeToAccessDate = model.FreeToAccessDate,
                Uploader = uploader,
                Language = language,
                Chapter = chapter
            };
            _db.WebNovelChapterContent.Add(chapterContent);
            await _db.SaveChangesAsync();
            
            return WebNovelAddChapterResult.Success;
        }
        
        public async Task<WebNovelChapter> GetChapterAsync(string webNovelUrl, int volume, int chapterNumber)
        {
            var webNovel = await _db.WebNovels
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            if (webNovel == null)
                return null;

            var chapter = _db.WebNovelChapters
                .Include(c => c.PreviousChapter)
                .Include(c => c.NextChapter)
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefault(c => c.WebNovel == webNovel &&
                                      (!webNovel.UsesVolumes || c.Volume == volume) &&
                                      c.Number == chapterNumber);
            
            if (chapter == null)
                return null;
            
            chapter.WebNovel = webNovel;
            return chapter;
        }
        
        public async Task<WebNovelChapterContent> GetChapterTranslationAsync(string webNovelUrl, string languageCode, int volume, int chapterNumber)
        {
            var chapter = await GetChapterAsync(webNovelUrl, volume, chapterNumber);
            var chapterContent = chapter?.Translations.SingleOrDefault(c => c.Language.Code == languageCode);
            
            if (chapterContent == null)
                return null;
            
            chapterContent.Chapter = chapter;
            return chapterContent;
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
                LastChapter = webNovel.GetLastChapter(),
                LastUpdateDate = webNovel.Chapters
                    .SelectMany(c => c.Translations)
                    .OrderByDescending(c => c.CreationDate)
                    .Select(c => c.CreationDate)
                    .FirstOrDefault(),
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
        TranslatedChapterNumberExists,
        LanguageNotExists,
        Success
    }
}