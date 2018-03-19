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
            
            builder.Entity<ForumThread>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.ForumThread)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Attachment>()
                .HasMany(c => c.Downloads)
                .WithOne(c => c.Attachment)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<News>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.News)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<News>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.News)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<NewsComment>()
                .HasMany(c => c.Children)
                .WithOne(c => c.Parent)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(c => c.Attachments)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasMany(c => c.Licenses)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Restrict); // Should manually review licenses before product deletion
            
            builder.Entity<Product>()
                .HasMany(c => c.SharedData)
                .WithOne(c => c.Product)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(builder);
        }
    }
}
