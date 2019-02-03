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
    }
}