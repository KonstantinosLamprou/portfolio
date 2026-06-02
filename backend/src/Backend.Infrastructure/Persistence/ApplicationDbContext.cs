using Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Backend.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Like> Likes { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Blog-Konfiguration
            builder.Entity<Blog>()
                .HasOne(b => b.Author)                    // Ein Blog hat EINEN Author
                .WithMany(b => b.CreatedBlogs)          // Ein User kann VIELE Blogs erstellen
                .HasForeignKey(b => b.AuthorId)           // Foreign Key ist AuthorId
                .OnDelete(DeleteBehavior.Restrict);       // Beim Löschen des Users: nicht löschen


            builder.Entity<Blog>()
                    .OwnsMany(b => b.Content, builder =>
                    {
                        // Speichert die komplette Liste als JSON in der Blog-Tabelle
                        builder.ToJson();
                    });

            builder.Entity<Project>()
                .HasOne(p => p.Author)
                .WithMany(p => p.CreatedProjects)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                    .OwnsMany(p => p.Content, builder =>
                    {
                        builder.ToJson();
                    });

            builder.Entity<Like>()
                .HasKey(cl => new { cl.ContentId, cl.UserId });

            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                // Kein automatisches Löschen aller Replies, wenn Parent gelöscht wird
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommentVote>()
                // Die Kombination aus UserId und CommentId ist einzigartig.
                .HasKey(cv => new { cv.UserId, cv.CommentId });

        }


    }
}
