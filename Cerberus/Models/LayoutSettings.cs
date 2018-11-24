namespace Cerberus.Models
{
    public class LayoutSettings
    {
        public LayoutSettings()
        {
            LibraryManager.Library("nestable")
                .AddJs("/lib/nestable/jquery.nestable.js")
                .AddJsMin("/lib/nestable/jquery.nestable.min.js");
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