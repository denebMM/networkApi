using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SocialNetwork.Models
{
    public partial class SOCIALDBContext : DbContext
    {
        public SOCIALDBContext()
        {
        }

        public SOCIALDBContext(DbContextOptions<SOCIALDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Like> Likes { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserFollower> UserFollowers { get; set; } = null!;
        public virtual DbSet<UserFriend> UserFriends { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.LikesNavigation)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Posts");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Users");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsPublic)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_USERID");
            });

            modelBuilder.Entity<UserFollower>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FollowerId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFollowers)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<UserFriend>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.FriendId });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserFriends)
                    .HasForeignKey(d => d.UserId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
