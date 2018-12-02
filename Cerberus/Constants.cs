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
            public const int ItemsPerIndexPage = 20;
        }
    }
}