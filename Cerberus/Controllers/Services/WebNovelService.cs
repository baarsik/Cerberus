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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Controllers.Services
{
    public sealed class WebNovelService : BaseService
    {
        public WebNovelService(ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
            : base(context, userManager, configuration)
        {
        }

        public async Task<WebNovelIndexViewModel> GetWebNovelIndexViewModelAsync(ApplicationUser user, int page)
        {
            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var webNovelsToDisplayCount = await Db.WebNovels.CountAsync(c => c.Translations.Any(d => languages.Contains(d.Language)));
            var totalPages = (int) Math.Ceiling(webNovelsToDisplayCount / (double) Constants.WebNovel.ItemsPerIndexPage);
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
            
            var webNovels = Db.WebNovels
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Where(c => c.Translations.Any(d => languages.Contains(d.Language)))
                .ToList();
            
            model.Items = webNovels
                .Select(webNovel => GetWebNovelInfo(webNovel, languages))
                .OrderByDescending(c => c.LastUpdateDate)
                .Take(Constants.WebNovel.ItemsPerIndexPage)
                .Skip(Constants.WebNovel.ItemsPerIndexPage * (model.Page - 1))
                .ToList();
            
            return model;
        }

        public async Task<AddWebNovelViewModel> GetAddWebNovelViewModelAsync(ApplicationUser user, string webNovelUrl)
        {
            var webNovel = await Db.WebNovels
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            if (webNovel == null)
                return null;
            
            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var translation = webNovel.GetTranslation(languages);

            if (translation == null)
                return null;
            
            return new AddWebNovelViewModel
            {
                UrlName = webNovel.UrlName,
                OriginalName = webNovel.OriginalName,
                Author = webNovel.Author,
                Name = translation.Name,
                Description = translation.Description,
                UsesVolumes = webNovel.UsesVolumes,
                Languages = await GetLanguagesAsync(webNovel.Translations.Select(c => c.Language).ToList())
            };
        }
        
        public async Task<WebNovelDetailsViewModel> GetWebNovelDetailsViewModelAsync(ApplicationUser user, string webNovelUrl)
        {
            var webNovel = await Db.WebNovels
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var model = new WebNovelDetailsViewModel
            {
                WebNovelInfo = GetWebNovelInfo(webNovel, languages),
                IsValid = webNovel != null
            };
            
            return model;
        }
        
        public async Task<AddChapterViewModel> GetWebNovelAddChapterViewModelAsync(ApplicationUser user, Guid webNovelId, AddChapterViewModel model = null)
        {
            if (model == null)
            {
                model = new AddChapterViewModel
                {
                    WebNovelId = webNovelId
                };
            }

            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            model.WebNovel = await GetWebNovelByIdAsync(model.WebNovelId);
            model.WebNovelContent = model.WebNovel.GetTranslation(languages);
            model.Languages = model.WebNovel.Translations.Select(c => c.Language).Distinct().ToList();

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
            return await Db.WebNovels
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<WebNovelAddWebNovelResult> AddWebNovelAsync(ApplicationUser user, AddWebNovelViewModel model)
        {
            var url = model.UrlName.ToLower();
            if (await Db.WebNovels.AnyAsync(c => c.UrlName.ToLower() == url))
                return WebNovelAddWebNovelResult.WebNovelUrlExists;
            
            var language = await Db.Languages.FirstOrDefaultAsync(c => c.Id == model.LanguageId);
            if (language == null)
                return WebNovelAddWebNovelResult.LanguageNotExists;
            
            var webNovel = new WebNovel
            {
                Id = Guid.NewGuid(),
                OriginalName = model.OriginalName.RemoveHTML(),
                UrlName = url.RemoveHTML(),
                UsesVolumes = model.UsesVolumes,
                Author = model.Author,
                CreationDate = DateTime.Now
            };
            Db.WebNovels.Add(webNovel);
            await Db.SaveChangesAsync();

            var webNovelContent = new WebNovelContent
            {
                Id = Guid.NewGuid(),
                Name = model.Name.RemoveHTML(),
                Description = model.Description.SanitizeHTML(),
                Language = language,
                WebNovel = webNovel
            };
            Db.WebNovelContent.Add(webNovelContent);
            await Db.SaveChangesAsync();
            
            return WebNovelAddWebNovelResult.Success;
        }
        
        public async Task<WebNovelAddWebNovelTranslationResult> AddWebNovelTranslationAsync(AddWebNovelViewModel model)
        {
            var url = model.UrlName.ToLower();
            var webNovel = await Db.WebNovels
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName.ToLower() == url);
            
            if (webNovel == null)
                return WebNovelAddWebNovelTranslationResult.WebNovelNotExists;
            
            var language = await Db.Languages.FirstOrDefaultAsync(c => c.Id == model.LanguageId);
            if (language == null)
                return WebNovelAddWebNovelTranslationResult.LanguageNotExists;

            if (webNovel.Translations.Any(c => c.Language == language))
                return WebNovelAddWebNovelTranslationResult.TranslationExists;

            var webNovelContent = new WebNovelContent
            {
                Id = Guid.NewGuid(),
                Name = model.Name.RemoveHTML(),
                Description = model.Description.SanitizeHTML(),
                Language = language,
                WebNovel = webNovel
            };
            Db.WebNovelContent.Add(webNovelContent);
            await Db.SaveChangesAsync();
            
            return WebNovelAddWebNovelTranslationResult.Success;
        }
        
        public async Task<WebNovelAddChapterResult> AddChapterAsync(ApplicationUser uploader, AddChapterViewModel model)
        {
            var webNovel = await Db.WebNovels
                .Include(c => c.Chapters)
                    .ThenInclude(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == model.WebNovelId);
            
            if (webNovel == null)
                return WebNovelAddChapterResult.WebNovelNotExists;

            if (!webNovel.UsesVolumes)
            {
                model.Volume = 1;
            }

            var language = await Db.Languages.FirstOrDefaultAsync(c => c.Id == model.LanguageId);
            
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
                Db.WebNovelChapters.Add(chapter);
                await Db.SaveChangesAsync();

                if (previousChapter != null)
                {
                    previousChapter.NextChapter = chapter;
                    Db.Update(previousChapter);
                }

                if (nextChapter != null)
                {
                    nextChapter.PreviousChapter = chapter;
                    Db.Update(nextChapter);
                }

                chapter.PreviousChapter = previousChapter;
                chapter.NextChapter = nextChapter;
                Db.Update(chapter);
                await Db.SaveChangesAsync();
            }

            var chapterContent = new WebNovelChapterContent
            {
                Id = Guid.NewGuid(),
                Title = model.Title.RemoveHTML(),
                Text = model.Text.SanitizeHTML(),
                CreationDate = DateTime.Now,
                FreeToAccessDate = DateTime.ParseExact(model.FreeToAccessDate, Constants.Misc.DateFormat, CultureInfo.InvariantCulture),
                Uploader = uploader,
                Language = language,
                Chapter = chapter
            };
            Db.WebNovelChapterContent.Add(chapterContent);
            await Db.SaveChangesAsync();
            
            return WebNovelAddChapterResult.Success;
        }
        
        public async Task<WebNovelChapter> GetChapterAsync(string webNovelUrl, int volume, int chapterNumber)
        {
            var webNovel = await Db.WebNovels
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            if (webNovel == null)
                return null;

            var chapter = Db.WebNovelChapters
                .Include(c => c.PreviousChapter)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.NextChapter)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.WebNovel)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefault(c => c.WebNovel == webNovel &&
                                      (!webNovel.UsesVolumes || c.Volume == volume) &&
                                      c.Number == chapterNumber);
            
            if (chapter == null)
                return null;
            
            chapter.WebNovel = webNovel;
            return chapter;
        }
        
        public async Task<WebNovelReadViewModel> GetChapterTranslationAsync(ApplicationUser user, string webNovelUrl, string languageCode, int volume, int chapterNumber)
        {
            var chapter = await GetChapterAsync(webNovelUrl, volume, chapterNumber);
            var chapterContent = chapter?.Translations.SingleOrDefault(c => c.Language.Code == languageCode);
            
            if (chapterContent == null)
                return null;
            
            chapterContent.Chapter = chapter;

            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var model = new WebNovelReadViewModel
            {
                WebNovelContent = chapter.WebNovel.Translations.SingleOrDefault(c => c.Language.Code == languageCode),
                Translation = chapterContent,
                NextChapterContent = chapter.NextChapter.GetTranslation(languages),
                PrevChapterContent = chapter.PreviousChapter.GetTranslation(languages)
            };
            
            return model;
        }

        private WebNovelInfo GetWebNovelInfo(WebNovel webNovel, ICollection<Language> languages)
        {
            if (webNovel?.Chapters == null)
            {
                return null;
            }

            var lastChapterTranslation = webNovel.GetLastChapterTranslation(languages);
            
            return new WebNovelInfo
            {
                WebNovel = webNovel,
                WebNovelContent = webNovel.GetTranslation(languages),
                TotalChapters = webNovel.Chapters.Count,
                LastChapterTranslation = lastChapterTranslation,
                LastUpdateDate = lastChapterTranslation == null ?
                    (DateTime?)null
                    : webNovel.Chapters
                        .SelectMany(c => c.Translations)
                        .OrderByDescending(c => c.CreationDate)
                        .Select(c => c.CreationDate)
                        .FirstOrDefault(),
                TotalVolumes = webNovel.Chapters
                    .Select(c => c.Volume)
                    .OrderByDescending(c => c)
                    .FirstOrDefault(),
                UserLanguages = languages
            };
        }
    }

    public enum WebNovelAddWebNovelResult
    {
        UnknownFailure,
        WebNovelUrlExists,
        LanguageNotExists,
        Success
    }
    
    public enum WebNovelAddWebNovelTranslationResult
    {
        UnknownFailure,
        WebNovelNotExists,
        LanguageNotExists,
        TranslationExists,
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