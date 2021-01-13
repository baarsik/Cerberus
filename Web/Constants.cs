namespace Web
{
    public static class Constants
    {
        /// <summary>
        /// User roles
        /// Tip: created automatically
        /// </summary>
        public static class Roles
        {
            public const string Admin = "admin";
            public const string GlobalModerator = "moderator";
            public const string WebNovelEditor = "webNovelEditor";
            public const string User = "user";
        }

        public static class Profile
        {
            public const int AvatarSize = 256;
        }

        public static class Permissions
        {
            public const string WebNovelEdit = Roles.Admin + "," + Roles.GlobalModerator + "," + Roles.WebNovelEditor;
        }

        public static class Notifications
        {
            public const int ItemsPerIndexPage = 25;
        }

        public static class WebNovel
        {
            public const int ItemsPerIndexPage = 10;
        }
        
        public static class Comments
        {
            public const int ItemsPerPage = 10;
        }

        public static class Misc
        {
            public const string DateFormat = "dd.MM.yyyy";
            public const string DateTimeFormat = "dd.MM.yyyy HH:mm";
            public const string DateFormatJs = "dd.mm.yyyy";
            public const string RandomStringSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }

        public static class Libraries
        {
            public const string Datepicker = "bootstrap-datepicker";
            public const string MomentJs = "moment";
            public const string NestableJs = "nestable";
            public const string Select2 = "select2";
        }
    }
}