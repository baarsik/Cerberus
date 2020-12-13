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
using Microsoft.Extensions.Configuration;

namespace Cerberus.Controllers.Services
{
    public sealed class WebNovelService : BaseService
    {
        private readonly NotificationsService _notificationsService;

        public WebNovelService(ApplicationContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            NotificationsService notificationsService)
            : base(context, userManager, configuration)
        {
            _notificationsService = notificationsService;
        }

        public async Task<WebNovelIndexViewModel> GetWebNovelIndexViewModelAsync(ApplicationUser user, int page)
        {
            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var webNovelsToDisplayCount = await Db.WebNovels.CountAsync(c => c.Translations.Any(d => languages.Any(l => Equals(l, d.Language))));
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
                .Where(c => c.Translations.Any(d => languages.Any(l => Equals(l, d.Language))))
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
                IsAdultContent = webNovel.IsAdultContent,
                UsesVolumes = webNovel.UsesVolumes,
                Languages = await GetLanguagesAsync(webNovel.Translations.Select(c => c.Language).ToList())
            };
        }

        public async Task<EditWebNovelViewModel> GetEditWebNovelViewModelAsync(Guid translationId)
        {
            var translation = await Db.WebNovelContent
                .Include(x => x.WebNovel)
                .Include(x => x.Language)
                .FirstOrDefaultAsync(x => x.Id == translationId);

            if (translation == null)
                return null;

            return new EditWebNovelViewModel
            {
                TranslationId = translation.Id,
                UrlName = translation.WebNovel.UrlName,
                OriginalName = translation.WebNovel.OriginalName,
                Author = translation.WebNovel.Author,
                Name = translation.Name,
                Description = translation.Description,
                IsAdultContent = translation.WebNovel.IsAdultContent,
                UsesVolumes = translation.WebNovel.UsesVolumes,
                LanguageName = translation.Language.LocalName
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
                .Include(c => c.ReaderData)
                    .ThenInclude(c => c.User)
                .Include(c => c.ReaderData)
                    .ThenInclude(c => c.LastOpenedChapter)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            var languages = user.GetUserOrDefaultLanguages(Db, Configuration);
            var readerData = webNovel?.ReaderData
                .Where(c => user != null && c.User.Id == user.Id)
                .Select(c => new WebNovelDetailsViewModel.ReaderUserData
                {
                    NotificationsEnabled = c.NotificationsEnabled,
                    LastOpenedChapter = c.LastOpenedChapter == null ? null : new WebNovelDetailsViewModel.ChapterLinkInfo
                    {
                        LanguageCode = languages.Where(c.LastOpenedChapter.Translations.Select(tl => tl.Language).Contains)
                            .Select(x => x.Code)
                            .FirstOrDefault(),
                        Volume = c.LastOpenedChapter.Volume,
                        Number = c.LastOpenedChapter.Number
                    },
                    SortOrder = c.SortOrder
                })
                .FirstOrDefault();
            
            var model = new WebNovelDetailsViewModel
            {
                WebNovelInfo = GetWebNovelInfo(webNovel, languages),
                ReaderData = readerData ?? new WebNovelDetailsViewModel.ReaderUserData(),
                IsValid = webNovel != null
            };
            
            return model;
        }
        
        public async Task<AddChapterViewModel> GetWebNovelAddChapterViewModelAsync(ApplicationUser user, Guid webNovelId)
        {
            var model = new AddChapterViewModel
            {
                WebNovelId = webNovelId,
                IsTranslation = false
            };

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
        
        public async Task<AddChapterViewModel> GetWebNovelAddChapterTranslationViewModelAsync(ApplicationUser user, Guid webNovelId, int chapterNumber)
        {
            var webNovelChapterContentId = await Db.WebNovelChapters
                .Where(c => c.WebNovel.Id == webNovelId && c.Number == chapterNumber)
                .SelectMany(c => c.Translations)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            return await GetWebNovelAddChapterTranslationViewModelAsync(user, webNovelChapterContentId);
        }
        
        public async Task<AddChapterViewModel> GetWebNovelAddChapterTranslationViewModelAsync(ApplicationUser user, Guid webNovelChapterContentId)
        {
            var chapterContent = await Db.WebNovelChapterContent
                .Include(c => c.Chapter)
                    .ThenInclude(c => c.WebNovel)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Chapter)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Where(c => c.Id == webNovelChapterContentId)
                .SingleOrDefaultAsync();

            if (chapterContent == null)
                return null;

            var chapterLanguages = chapterContent.Chapter.Translations.Select(c => c.Language).ToList();
            var webNovelLanguages = chapterContent.Chapter.WebNovel.Translations.Select(c => c.Language).ToList();
            
            var model = await GetWebNovelAddChapterViewModelAsync(user, chapterContent.Chapter.WebNovel.Id);
            model.IsTranslation = true;
            model.Volume = chapterContent.Chapter.Volume;
            model.Number = chapterContent.Chapter.Number;
            model.Title = chapterContent.Title;
            model.Text = chapterContent.Text;
            model.Languages = model.Languages
                .Where(c => webNovelLanguages.Contains(c) && !chapterLanguages.Contains(c))
                .ToList();
            model.LanguageId = model.Languages
                .Select(c => c.Id)
                .FirstOrDefault();
            
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
                IsAdultContent = model.IsAdultContent,
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

            if (webNovel.Translations.Any(c => Equals(c.Language, language)))
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

        public async Task<WebNovelAddChapterResult> AddChapterContentAsync(ApplicationUser uploader, AddChapterViewModel model)
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
            
            if (chapter != null && chapter.Translations.Any(c => Equals(c.Language, language)))
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
                    IsAdultContent = model.IsAdultContent,
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

            if (chapter.IsAdultContent != model.IsAdultContent)
            {
                chapter.IsAdultContent = model.IsAdultContent;
            }

            var chapterContent = new WebNovelChapterContent
            {
                Id = Guid.NewGuid(),
                Title = model.Title.RemoveHTML(),
                Text = model.Text.SanitizeHTML(),
                CreationDate = DateTime.Now,
                FreeToAccessDate = DateTime.ParseExact(model.FreeToAccessDate, Constants.Misc.DateFormat, CultureInfo.InvariantCulture),
                Symbols = model.Text.GetPureTextLength(),
                Uploader = uploader,
                Language = language,
                Chapter = chapter
            };
            Db.WebNovelChapterContent.Add(chapterContent);
            await Db.SaveChangesAsync();

            await UpdateWebNovelSymbolCountAsync(webNovel.Id, language.Id);
            await _notificationsService.AddNewWebNovelChapterNotificationAsync(chapterContent);
            
            return WebNovelAddChapterResult.Success;
        }

        public async Task<WebNovelEditWebNovelTranslationResult> EditWebNovelTranslationAsync(EditWebNovelViewModel model)
        {
            var translation = await Db.WebNovelContent
                .Include(x => x.WebNovel)
                .SingleOrDefaultAsync(x => x.Id == model.TranslationId);

            if (translation == null)
                return WebNovelEditWebNovelTranslationResult.TranslationNotExists;

            translation.Name = model.Name;
            translation.Description = model.Description;
            Db.Update(translation);

            var webNovel = translation.WebNovel;
            webNovel.IsAdultContent = model.IsAdultContent;
            webNovel.UsesVolumes |= model.UsesVolumes;
            Db.Update(webNovel);
            await Db.SaveChangesAsync();

            return WebNovelEditWebNovelTranslationResult.Success;
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
            if (user == null)
            {
                languages = languages
                    .OrderByDescending(x => x.Code == languageCode)
                    .ToList();
            }
            var model = new WebNovelReadViewModel
            {
                WebNovelContent = chapter.WebNovel.Translations.SingleOrDefault(c => c.Language.Code == languageCode),
                Translation = chapterContent,
                NextChapterContent = chapter.NextChapter.GetTranslation(languages),
                PrevChapterContent = chapter.PreviousChapter.GetTranslation(languages)
            };
            
            return model;
        }

        public async Task UpdateLastReadChapterAsync(ApplicationUser user, WebNovelChapter chapter)
        {
            if (user == null)
                return;
            
            var webNovel = await Db.WebNovels
                .Include(c => c.ReaderData)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(x => x.Chapters.Contains(chapter));
            
            var readerData = webNovel.ReaderData.FirstOrDefault(x => x.User.Id == user.Id);
            if (readerData == null)
            {
                Db.Add(new WebNovelReaderData
                {
                    User = user,
                    WebNovel = webNovel,
                    LastOpenedChapter = chapter
                });
            }
            else
            {
                readerData.LastOpenedChapter = chapter;
                Db.Update(readerData);
            }
            await Db.SaveChangesAsync();
        }

        public async Task<EditChapterTranslationViewModel> GetEditChapterTranslationViewModelAsync(ApplicationUser user, Guid id)
        {
            var content = await Db.WebNovelChapterContent
                .Include(c => c.Language)
                .Include(c => c.Chapter)
                    .ThenInclude(c => c.WebNovel)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Include(c => c.Chapter)
                    .ThenInclude(c => c.Translations)
                    .ThenInclude(c => c.Language)
                .Where(c => c.Id == id)
                .SingleOrDefaultAsync();
            
            if (content == null)
                return null;
            
            var model = new EditChapterTranslationViewModel
            {
                WebNovelChapterContentId = content.Id,
                Volume = content.Chapter.Volume,
                Number = content.Chapter.Number,
                Title = content.Title,
                Text = content.Text,
                IsAdultContent = content.Chapter.IsAdultContent,
                FreeToAccessDate = content.FreeToAccessDate.ToString(Constants.Misc.DateFormat),
                LanguageId = content.Language.Id,
                WebNovel = content.Chapter.WebNovel,
                WebNovelContent = content.Chapter.WebNovel.Translations.FirstOrDefault(c => Equals(c.Language, content.Language)),
                Languages = user.GetUserOrDefaultLanguages(Db, Configuration)
                    .Where(lang => Equals(lang, content.Language) || content.Chapter.Translations.All(t => !Equals(t.Language, lang)))
                    .ToList()
            };
            return model;
        }
        
        public async Task UpdateChapterContentAsync(ApplicationUser uploader, EditChapterTranslationViewModel model)
        {
            var chapterContent = await Db.WebNovelChapterContent
                .Include(c => c.Chapter)
                    .ThenInclude(c => c.WebNovel)
                .FirstOrDefaultAsync(c => c.Id == model.WebNovelChapterContentId);
            
            if (chapterContent == null)
                return;

            if (chapterContent.Chapter.IsAdultContent != model.IsAdultContent)
            {
                chapterContent.Chapter.IsAdultContent = model.IsAdultContent;
                Db.WebNovelChapters.Update(chapterContent.Chapter);
            }
            
            chapterContent.Title = model.Title.RemoveHTML();
            chapterContent.Text = model.Text.SanitizeHTML();
            chapterContent.Symbols = model.Text.GetPureTextLength();
            chapterContent.FreeToAccessDate = DateTime.ParseExact(model.FreeToAccessDate, Constants.Misc.DateFormat, CultureInfo.InvariantCulture);
            chapterContent.Language = await Db.Languages.FindAsync(model.LanguageId);
            
            Db.WebNovelChapterContent.Update(chapterContent);
            await Db.SaveChangesAsync();

            await UpdateWebNovelSymbolCountAsync(chapterContent.Chapter.WebNovel.Id, model.LanguageId);
        }

        public async Task UpdateNotificationStatus(ApplicationUser user, string webNovelUrl, bool notificationsEnabled)
        {
            var webNovel = await Db.WebNovels
                .Include(x=> x.ReaderData)
                    .ThenInclude(x => x.User)
                .SingleOrDefaultAsync(c => c.UrlName == webNovelUrl.ToLower(CultureInfo.InvariantCulture));

            var readerData = webNovel.ReaderData.FirstOrDefault(x => x.User.Id == user.Id);
            if (readerData == null)
            {
                Db.Add(new WebNovelReaderData
                {
                    User = user,
                    WebNovel = webNovel,
                    NotificationsEnabled = notificationsEnabled
                });
            }
            else
            {
                readerData.NotificationsEnabled = notificationsEnabled;
                Db.Update(readerData);
            }
            await Db.SaveChangesAsync();
        }

        private WebNovelInfo GetWebNovelInfo(WebNovel webNovel, IList<Language> languages)
        {
            if (webNovel?.Chapters == null)
            {
                return null;
            }

            var lastChapter = webNovel.GetLastChapter(languages);
            var lastChapterTranslation = webNovel.GetLastChapterTranslation(languages);
            
            return new WebNovelInfo
            {
                WebNovel = webNovel,
                WebNovelContent = webNovel.GetTranslation(languages),
                TotalChapters = webNovel.Chapters
                    .Count(c => c.Translations.Any(t => languages.Any(l => Equals(l, t.Language)))),
                LastChapterTranslation = lastChapter,
                LastUpdateDate = lastChapterTranslation == null ?
                    (DateTime?)null
                    : webNovel.Chapters
                        .SelectMany(c => c.Translations)
                        .Where(c => languages.Contains(c.Language))
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
        
        private async Task UpdateWebNovelSymbolCountAsync(Guid webNovelId, Guid languageId)
        {
            var webNovelContent = await Db.WebNovelContent
                .FirstOrDefaultAsync(x =>
                    x.WebNovel.Id == webNovelId &&
                    x.Language.Id == languageId);
            
            webNovelContent.Symbols = await Db.WebNovelChapterContent
                .Where(x =>
                    x.Language.Id == webNovelContent.Language.Id &&
                    x.Chapter.WebNovel.Id == webNovelContent.WebNovel.Id)
                .SumAsync(x => x.Symbols);
            Db.Update(webNovelContent);
            await Db.SaveChangesAsync();
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

    public enum WebNovelEditWebNovelTranslationResult
    {
        UnknownFailure,
        TranslationNotExists,
        Success
    }
}