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
        
        public DbSet<News> News { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumThreadReply> ForumThreadReplies { get; set; }
        public DbSet<ForumModerator> ForumModerators { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentDownloads> AttachmentDownloads { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductSettings> ProductSettings { get; set; }
        public DbSet<ProductLicense> ProductLicenses { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<WebNovel> WebNovels { get; set; }
        public DbSet<WebNovelContent> WebNovelContent { get; set; }
        public DbSet<WebNovelChapter> WebNovelChapters { get; set; }
        public DbSet<WebNovelChapterAccess> WebNovelChapterAccess { get; set; }
        public DbSet<WebNovelChapterContent> WebNovelChapterContent { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ApplicationUserLanguage> UserLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Forum>()
                .HasQueryFilter(c => c.IsEnabled);
            
            builder.Entity<NewsComment>()
                .HasQueryFilter(c => !c.IsDeleted);
            
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
            
            builder.Entity<ForumThread>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.ForumThread)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Attachment>()
                .HasMany(c => c.Downloads)
                .WithOne(c => c.Attachment)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<News>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.News)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<News>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.News)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NewsComment>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasMany(c => c.Licenses)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.Entity<Product>()
                .HasMany(c => c.SharedData)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<WebNovel>()
                .HasIndex(c => c.UrlName)
                .IsUnique();
            
            builder.Entity<WebNovel>()
                .HasOne(c => c.Country)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
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
            
            builder.Entity<ApplicationUserLanguage>()
                .HasOne(c => c.User)
                .WithMany(c => c.UserLanguages)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(builder);
        }
    }
}
