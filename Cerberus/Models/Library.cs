using System.Collections.Generic;
using System.Linq;

namespace Cerberus.Models
{
    public class Library
    {
        public Library(string name)
        {
            Name = name;
        }
         
        public string Name { get; }
        public bool IsActive { get; private set; }
        public bool IsEmpty => !(CssFiles.Any() || CssFilesMinimized.Any() || JsFiles.Any() || JsFilesMinimized.Any());

        public ICollection<string> CssFiles { get; } = new List<string>();
        public ICollection<string> CssFilesMinimized { get; } = new List<string>();

        public ICollection<string> JsFiles { get; } = new List<string>();
        public ICollection<string> JsFilesMinimized { get; } = new List<string>();
         
        public Library AddCss(string path)
        {
            CssFiles.Add(path);
            return this;
        }
         
        public Library AddCssMin(string path)
        {
            CssFilesMinimized.Add(path);
            return this;
        }
         
        public Library AddJs(string path)
        {
            JsFiles.Add(path);
            return this;
        }
         
        public Library AddJsMin(string path)
        {
            JsFilesMinimized.Add(path);
            return this;
        }
         
        public void Activate()
        {
            IsActive = true;
        }
    }
}