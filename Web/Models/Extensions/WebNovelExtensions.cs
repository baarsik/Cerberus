using System.Collections.Generic;
using System.Linq;
using DataContext.Models;

namespace Web.Models.Extensions
{
    public static class WebNovelExtensions
    {
        public static WebNovelChapter GetLastChapter(this WebNovel webNovel)
        {
            return webNovel.Chapters
                .OrderByDescending(c => c.Volume)
                .ThenByDescending(c => c.Number)
                .FirstOrDefault();
        }
        
        public static WebNovelContent GetTranslation(this WebNovel webNovel, IList<Language> languages)
        {
            return webNovel?.Translations
                .Where(c => languages.Contains(c.Language))
                .OrderBy(c => languages.IndexOf(c.Language))
                .FirstOrDefault();
        }

        public static int GetNumberOfSymbols(this WebNovel webNovel, IList<Language> languages)
        {
            return webNovel.Chapters
                .SelectMany(x => x.Translations)
                .Where(x => languages.Contains(x.Language))
                .OrderBy(x => languages.IndexOf(x.Language))
                .GroupBy(x => x.ChapterId)
                .Select(x => x.First())
                .Sum(x => x.Symbols);

        }
        
        public static WebNovelChapterContent GetLastChapterContent(this WebNovel webNovel, IList<Language> languages)
        {
            return webNovel?.Chapters
                .Where(c => c.Translations.Any(d => languages.Contains(d.Language)))
                .OrderByDescending(c => c.Volume)
                .ThenByDescending(c => c.Number)
                .SelectMany(c => c.Translations)
                .Where(c => languages.Contains(c.Language))
                .OrderByDescending(c => c.Chapter.Volume)
                .ThenByDescending(c => c.Chapter.Number)
                .ThenBy(c => languages.IndexOf(c.Language))
                .FirstOrDefault();
        }
        
        public static WebNovelChapterContent GetLastChapterTranslation(this WebNovel webNovel, IList<Language> languages)
        {
            return webNovel?.Chapters
                .SelectMany(c => c.Translations)
                .Where(c => languages.Contains(c.Language))
                .OrderBy(c => languages.IndexOf(c.Language))
                .ThenByDescending(c => c.CreationDate)
                .FirstOrDefault();
        }
    }
}