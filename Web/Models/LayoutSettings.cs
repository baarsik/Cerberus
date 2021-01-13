namespace Web.Models
{
    public class LayoutSettings
    {
        public LayoutSettings()
        {
            LibraryManager.Library(Constants.Libraries.Datepicker)
                .AddJs("/lib/bootstrap-datepicker/bootstrap-datepicker.js")
                .AddJsMin("/lib/bootstrap-datepicker/bootstrap-datepicker.min.js");
            
            LibraryManager.Library(Constants.Libraries.MomentJs)
                .AddJs("/lib/moment/moment.js")
                .AddJsMin("/lib/moment/min/moment.min.js");
            
            LibraryManager.Library(Constants.Libraries.NestableJs)
                .AddJs("/lib/nestable/jquery.nestable.js")
                .AddJsMin("/lib/nestable/jquery.nestable.min.js");

            LibraryManager.Library(Constants.Libraries.Select2)
                .AddJs("/lib/select2/dist/js/select2.full.js")
                .AddJsMin("/lib/select2/dist/js/select2.full.min.js")
                .AddCss("/lib/select2/dist/css/select2.css")
                .AddCssMin("/lib/select2/dist/css/select2.min.css");
        }

        public void ActivateLibrary(string name)
        {
            LibraryManager.Library(name).Activate(LibraryManager);
        }
        
        public bool IsContainerCentered { get; set; }
        public bool IsFooterVisible { get; set; } = true;
        public NavbarType NavbarType { get; set; } = NavbarType.Regular;
        public LibraryManager LibraryManager { get; } = new();
    }

    public enum NavbarType
    {
        None,
        Admin,
        Regular
    }
}