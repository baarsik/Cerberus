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
            return GetOrderedLibraries().SelectMany(c => c.CssFiles);
        }
         
        public IEnumerable<string> GetCssFilesMinimized()
        {
            return GetOrderedLibraries().SelectMany(c => c.CssFilesMinimized);
        }
         
        public IEnumerable<string> GetJsFiles()
        {
            return GetOrderedLibraries().SelectMany(c => c.JsFiles);
        }
         
        public IEnumerable<string> GetJsFilesMinimized()
        {
            return GetOrderedLibraries().SelectMany(c => c.JsFilesMinimized);
        }

        private IEnumerable<Library> GetOrderedLibraries()
        {
            var libs = Libraries.Where(c => c.IsActive).ToList();

            while (libs.Any())
            {
                foreach (var lib in libs.Where(c =>
                    !c.Dependencies.Any(libName => libs.Any(library => library.Name == libName))))
                {
                    yield return lib;
                    libs.Remove(lib);
                    break;
                }
            }
        }
    }
}