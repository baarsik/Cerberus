namespace Cerberus.Models
{
    public class LayoutSettings
    {
        public LayoutSettings()
        {
            LibraryManager.Library(Constants.Libraries.NestableJs)
                .AddJs("/lib/nestable/jquery.nestable.js")
                .AddJsMin("/lib/nestable/jquery.nestable.min.js");

            LibraryManager.Library(Constants.Libraries.Summernote)
                .AddJs("/lib/summernote/dist/summernote.js")
                .AddJsMin("/lib/summernote/dist/summernote.min.js")
                .AddCss("/lib/summernote/dist/summernote.css")
                .AddCssMin("/lib/summernote/dist/summernote.min.css");
        }
        
        public bool IsContainerCentered { get; set; }
        public bool IsFooterVisible { get; set; } = true;
        public NavbarType NavbarType { get; set; } = NavbarType.Regular;
        public LibraryManager LibraryManager { get; set; } = new LibraryManager();
    }

    public enum NavbarType
    {
        None,
        Admin,
        Regular
    }
}