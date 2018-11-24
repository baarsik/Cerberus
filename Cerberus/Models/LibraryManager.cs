using System.Collections.Generic;
using System.Linq;

namespace Cerberus.Models
{
    public class LibraryManager
    {
        private readonly ICollection<Library> _libraries = new List<Library>();
         
        public IEnumerable<Library> Libraries => _libraries;

        public Library Library(string name)
        {
            name = name.ToLower();
            
            var library = _libraries.FirstOrDefault(c => c.Name == name);
            if (library != null)
                return library;
            
            library = new Library(name);
            _libraries.Add(library);
            return library;
        }

        public IEnumerable<string> GetCssFiles()
        {
            return Libraries.Where(c => c.IsActive).SelectMany(c => c.CssFiles);
        }
         
        public IEnumerable<string> GetCssFilesMinimized()
        {
            return Libraries.Where(c => c.IsActive).SelectMany(c => c.CssFilesMinimized);
        }
         
        public IEnumerable<string> GetJsFiles()
        {
            return Libraries.Where(c => c.IsActive).SelectMany(c => c.JsFiles);
        }
         
        public IEnumerable<string> GetJsFilesMinimized()
        {
            return Libraries.Where(c => c.IsActive).SelectMany(c => c.JsFilesMinimized);
        }
    }
}