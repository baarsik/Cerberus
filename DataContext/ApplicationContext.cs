using DataContext.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataContext
{
    public sealed class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<News> News { get; set; }
        public DbSet<NewsComment> NewsComments { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<ForumThread> ForumThreads { get; set; }
        public DbSet<ForumThreadReply> ForumThreadReplies { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentDownloads> AttachmentDownloads { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Forum>()
                   .HasIndex(c => c.DisplayOrderId)
                   .IsUnique();

            builder.Entity<AttachmentDownloads>()
                   .HasOne(c => c.Attachment)
                   .WithMany(c => c.Downloads)
                   .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql("Server=localhost;Uid=root;Pwd=29706430aA;Port=3306;database=cerberus;");
    }
}
