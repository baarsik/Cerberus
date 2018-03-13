#define USE_CONNECTION_STRING

using DataContext.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public sealed class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
#if USE_CONNECTION_STRING
        public ApplicationContext(DbContextOptions options) 
            : base(options)
        { }
#endif
        
        public DbSet<News> News { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumThreadReply> ForumThreadReplies { get; set; }
        public DbSet<ForumModerator> ForumModerators { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentDownloads> AttachmentDownloads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Forum>()
                .HasMany(c => c.Threads)
                .WithOne(c => c.Forum)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Forum>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<Forum>()
                .HasMany(c => c.Moderators)
                .WithOne(c => c.Forum)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ForumThread>()
                .HasMany(c => c.Replies)
                .WithOne(c => c.Thread)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attachment>()
                .HasMany(c => c.Downloads)
                .WithOne(c => c.Attachment)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<News>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.News)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(builder);
        }
        
#if !USE_CONNECTION_STRING
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)		
            => optionsBuilder.UseMySql("Server=localhost;Uid=root;Pwd=29706430aA;Port=3306;database=cerberus;");
#endif
    }
}
