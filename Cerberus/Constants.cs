namespace Cerberus
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
            public const string Moderator = "moderator";
            public const string WebNovelEditor = "webNovelEditor";
            public const string User = "user";
        }

        public static class WebNovel
        {
            public const int ItemsPerIndexPage = 10;
        }

        public static class Misc
        {
            public const string DateTimeFormat = "dd.MM.yyyy";
            public const string RandomStringSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        }

        public static class Libraries
        {
            public const string NestableJs = "nestable";
            public const string Summernote = "summernote";
        }
    }
}