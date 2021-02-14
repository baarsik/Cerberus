namespace Web.Models.ViewModels
{
    public class ProfileStatistics
    {
        public ProfileForumStatistics Forum { get; set; }
        public ProfileWebNovelStatistics WebNovel { get; set; }
    }

    public class ProfileForumStatistics
    {
        public int TotalStartedThreads { get; set; }
        public int TotalReplies { get; set; }
    }

    public class ProfileWebNovelStatistics
    {
        public int TotalWebNovels { get; set; }
        public int TotalChapters { get; set; }
        public int TotalComments { get; set; }
    }
}