using System.Collections.Generic;
using System.Linq;
using DataContext.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cerberus.Models.Extensions
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
        
        public static WebNovelContent GetTranslation(this WebNovel webNovel, ICollection<Language> languages)
        {
            return webNovel?.Translations
                .Where(c => languages.Contains(c.Language))
                .OrderBy(c => languages.IndexOf(c.Language))
                .FirstOrDefault();
        }
        
        public static WebNovelChapterContent GetLastChapter(this WebNovel webNovel, ICollection<Language> languages)
        {
            return webNovel?.Chapters
                .Where(c => c.Translations.Any(d => languages.Contains(d.Language)))
                .OrderByDescending(c => c.Volume)
                .ThenByDescending(c => c.Number)
                .SelectMany(c => c.Translations)
                .Where(c => languages.Contains(c.Language))
                .OrderByDescending(c => languages.IndexOf(c.Language))
                .ThenByDescending(c => c.CreationDate)
                .FirstOrDefault();
        }
        
        public static WebNovelChapterContent GetLastChapterTranslation(this WebNovel webNovel, ICollection<Language> languages)
        {
            return webNovel?.Chapters
                .SelectMany(c => c.Translations)
                .Where(c => languages.Contains(c.Language))
                .OrderByDescending(c => languages.IndexOf(c.Language))
                .ThenByDescending(c => c.CreationDate)
                .FirstOrDefault();
        }
    }
}