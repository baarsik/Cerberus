namespace Cerberus.Models
{
    public class LayoutSettings
    {
        public bool IsContainerCentered { get; set; }
        public bool IsFooterVisible { get; set; } = true;
        public NavbarType NavbarType { get; set; } = NavbarType.Regular;
    }

    public enum NavbarType
    {
        None,
        Admin,
        Regular
    }
}