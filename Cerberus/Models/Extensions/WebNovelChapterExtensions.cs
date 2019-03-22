using System.Collections.Generic;
using System.Linq;
using DataContext.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace Cerberus.Models.Extensions
{
    public static class WebNovelChapterExtensions
    {
        public static WebNovelChapterContent GetTranslation(this WebNovelChapter chapter, ICollection<Language> languages)
        {
            return chapter?.Translations
                .Where(c => languages.Contains(c.Language))
                .OrderBy(c => languages.IndexOf(c.Language))
                .FirstOrDefault();
        }
    }
}