﻿// <auto-generated />
using System;
using DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataContext.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataContext.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ApiBindedIp")
                        .HasMaxLength(15);

                    b.Property<string>("ApiKey")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Avatar")
                        .IsRequired();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Culture")
                        .HasMaxLength(8);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<Guid>("LastApiTokenId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("LockoutReason")
                        .HasMaxLength(100);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegisterDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DataContext.Models.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreateDate");

                    b.Property<Guid?>("ForumThreadId");

                    b.Property<Guid?>("NewsId");

                    b.Property<Guid?>("ProductId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UploaderId")
                        .IsRequired();

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ForumThreadId");

                    b.HasIndex("NewsId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("DataContext.Models.AttachmentDownloads", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AttachmentId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AttachmentId");

                    b.HasIndex("UserId");

                    b.ToTable("AttachmentDownloads");
                });

            modelBuilder.Entity("DataContext.Models.Forum", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DisplayOrderId");

                    b.Property<bool>("IsEnabled");

                    b.Property<Guid?>("ParentId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Forums");
                });

            modelBuilder.Entity("DataContext.Models.ForumModerator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ForumId");

                    b.Property<int>("Level");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ForumId");

                    b.HasIndex("UserId");

                    b.ToTable("ForumModerators");
                });

            modelBuilder.Entity("DataContext.Models.ForumThread", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<Guid>("ForumId");

                    b.Property<bool>("IsClosed");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPinned");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ForumId");

                    b.ToTable("ForumThreads");
                });

            modelBuilder.Entity("DataContext.Models.ForumThreadReply", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsMainReply");

                    b.Property<DateTime?>("LastEditDate");

                    b.Property<Guid>("ThreadId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ThreadId");

                    b.ToTable("ForumThreadReplies");
                });

            modelBuilder.Entity("DataContext.Models.News", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastEditDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Uri")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("News");
                });

            modelBuilder.Entity("DataContext.Models.NewsComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorId")
                        .IsRequired();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime?>("LastEditDate");

                    b.Property<Guid>("NewsId");

                    b.Property<Guid?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("NewsId");

                    b.HasIndex("ParentId");

                    b.ToTable("NewsComments");
                });

            modelBuilder.Entity("DataContext.Models.PrivateMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("DateTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsRead");

                    b.Property<bool>("IsSentByAdministration");

                    b.Property<string>("ReceiverId")
                        .IsRequired();

                    b.Property<string>("SenderId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("PrivateMessages");
                });

            modelBuilder.Entity("DataContext.Models.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<bool>("RequiresLicense");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("DataContext.Models.ProductLicense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsBlocked");

                    b.Property<Guid>("ProductId");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductLicenses");
                });

            modelBuilder.Entity("DataContext.Models.ProductSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<Guid>("ProductId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductSettings");
                });

            modelBuilder.Entity("DataContext.Models.ProductSharedData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<Guid>("ProductId");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSharedData");
                });

            modelBuilder.Entity("DataContext.Models.Settings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("DataContext.Models.WebNovel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverUrl");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<Guid?>("FirstChapterId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OriginalName");

                    b.Property<string>("UrlName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FirstChapterId");

                    b.HasIndex("UrlName")
                        .IsUnique();

                    b.ToTable("WebNovels");
                });

            modelBuilder.Entity("DataContext.Models.WebNovelChapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("FreeToAccessDate");

                    b.Property<Guid?>("NextChapterId");

                    b.Property<int>("Number");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("NextChapterId")
                        .IsUnique()
                        .HasFilter("[NextChapterId] IS NOT NULL");

                    b.ToTable("WebNovelChapters");
                });

            modelBuilder.Entity("DataContext.Models.WebNovelChapterAccess", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AccessPurchaseDateTime");

                    b.Property<Guid>("ChapterId");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.HasIndex("UserId");

                    b.ToTable("WebNovelChapterAccess");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DataContext.Models.Attachment", b =>
                {
                    b.HasOne("DataContext.Models.ForumThread", "ForumThread")
                        .WithMany("Attachments")
                        .HasForeignKey("ForumThreadId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.News", "News")
                        .WithMany("Attachments")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.Product", "Product")
                        .WithMany("Attachments")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.ApplicationUser", "Uploader")
                        .WithMany()
                        .HasForeignKey("UploaderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.Models.AttachmentDownloads", b =>
                {
                    b.HasOne("DataContext.Models.Attachment", "Attachment")
                        .WithMany("Downloads")
                        .HasForeignKey("AttachmentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.Models.Forum", b =>
                {
                    b.HasOne("DataContext.Models.Forum", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DataContext.Models.ForumModerator", b =>
                {
                    b.HasOne("DataContext.Models.Forum", "Forum")
                        .WithMany("Moderators")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DataContext.Models.ForumThread", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.Forum", "Forum")
                        .WithMany("Threads")
                        .HasForeignKey("ForumId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DataContext.Models.ForumThreadReply", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.ForumThread", "Thread")
                        .WithMany("Replies")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DataContext.Models.News", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.Models.NewsComment", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.News", "News")
                        .WithMany("Comments")
                        .HasForeignKey("NewsId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.NewsComment", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DataContext.Models.PrivateMessage", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");
                });

            modelBuilder.Entity("DataContext.Models.ProductLicense", b =>
                {
                    b.HasOne("DataContext.Models.Product", "Product")
                        .WithMany("Licenses")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DataContext.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.Models.ProductSettings", b =>
                {
                    b.HasOne("DataContext.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DataContext.Models.ProductSharedData", b =>
                {
                    b.HasOne("DataContext.Models.Product", "Product")
                        .WithMany("SharedData")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DataContext.Models.WebNovel", b =>
                {
                    b.HasOne("DataContext.Models.WebNovelChapter", "FirstChapter")
                        .WithMany()
                        .HasForeignKey("FirstChapterId");
                });

            modelBuilder.Entity("DataContext.Models.WebNovelChapter", b =>
                {
                    b.HasOne("DataContext.Models.WebNovelChapter", "NextChapter")
                        .WithOne("PreviousChapter")
                        .HasForeignKey("DataContext.Models.WebNovelChapter", "NextChapterId");
                });

            modelBuilder.Entity("DataContext.Models.WebNovelChapterAccess", b =>
                {
                    b.HasOne("DataContext.Models.WebNovelChapter", "Chapter")
                        .WithMany()
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataContext.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DataContext.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
