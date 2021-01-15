using DataContext.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public sealed class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions options) 
            : base(options)
        { }
        
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumThreadReply> ForumThreadReplies { get; set; }
        public DbSet<ForumModerator> ForumModerators { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<WebNovel> WebNovels { get; set; }
        public DbSet<WebNovelContent> WebNovelContent { get; set; }
        public DbSet<WebNovelChapter> WebNovelChapters { get; set; }
        public DbSet<WebNovelChapterAccess> WebNovelChapterAccess { get; set; }
        public DbSet<WebNovelChapterContent> WebNovelChapterContent { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ApplicationUserLanguage> UserLanguages { get; set; }
        public DbSet<ApplicationUserNotifications> ApplicationUserNotifications { get; set; }
        public DbSet<WebNovelTag> WebNovelTag { get; set; }
        public DbSet<WebNovelReaderData> WebNovelReaderData { get; set; }
        public DbSet<WebNovelChapterContentReaderData> WebNovelChapterContentReaderData { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Forum>()
                .HasQueryFilter(c => c.IsEnabled);
            
            builder.Entity<ForumThreadReply>()
                .HasQueryFilter(c => !c.IsDeleted);
            
            builder.Entity<Forum>()
                .HasMany(c => c.Threads)
                .WithOne(c => c.Forum)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Forum>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Forum>()
                .HasMany(c => c.Moderators)
                .WithOne(c => c.Forum)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ForumThread>()
                .HasMany(c => c.Replies)
                .WithOne(c => c.Thread)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WebNovel>()
                .HasIndex(c => c.UrlName)
                .IsUnique();
            
            builder.Entity<WebNovel>()
                .HasOne(c => c.Country)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<WebNovel>()
                .HasMany(c => c.ReaderData)
                .WithOne(c => c.WebNovel)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<WebNovelContent>()
                .HasOne(c => c.Language)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<WebNovelContent>()
                .HasOne(c => c.WebNovel)
                .WithMany(c => c.Translations)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WebNovelChapterContent>()
                .HasOne(c => c.Uploader)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WebNovelChapterContent>()
                .HasOne(c => c.Language)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<WebNovelChapterContent>()
                .HasOne(c => c.Chapter)
                .WithMany(c => c.Translations)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<WebNovelChapterContent>()
                .HasMany(c => c.ReaderData)
                .WithOne(c => c.ChapterContent)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUserLanguage>()
                .HasOne(c => c.User)
                .WithMany(c => c.UserLanguages)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WebNovelChapter>()
                .HasOne(c => c.NextChapter)
                .WithOne(c => c.PreviousChapter)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.WebNovelReaderData)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.WebNovelChapterContentReaderData)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Author)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.Notifications)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<ApplicationUser>()
                .HasMany(c => c.CoinUpdates)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUserNotifications>()
                .HasQueryFilter(c => !c.IsDeleted);
            
            builder.Entity<Comment>()
                .HasQueryFilter(c => !c.IsDeleted);
            
            base.OnModelCreating(builder);
        }
    }
}
