using System.Collections.Generic;
using System.Linq;
using DataContext.Models;

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
        
        public static WebNovelChapterContent GetLastChapterTranslation(this WebNovel webNovel, ICollection<Language> languages)
        {
            var chapter = webNovel.Chapters
                .Where(c => c.Translations.Any(d => languages.Contains(d.Language)))
                .OrderByDescending(c => c.Volume)
                .ThenByDescending(c => c.Number)
                .FirstOrDefault();

            if (chapter == null)
                return null;
            
            foreach (var language in languages)
            {
                var translation = chapter.Translations.FirstOrDefault(c => c.Language == language);
                if (translation == null)
                    continue;

                return translation;
            }

            return null;
        }
    }
}