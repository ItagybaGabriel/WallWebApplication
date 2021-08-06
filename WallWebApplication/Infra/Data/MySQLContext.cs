using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication
{
    public class MySQLContext : IdentityDbContext<ApplicationUser>
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
        {
        }
        public DbSet<Post> Post { get; set; }
        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<ApplicationRole> Role { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Chat> Chat { get; set; }


        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            modelBuilder.Entity<Like>()
                .HasOne<Post>(s => s.Post)
                .WithMany(g => g.Likes)
                //.HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Post>()
                .HasOne<ApplicationUser>(s => s.User)
                .WithMany(g => g.Posts)
                //.HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne<ApplicationUser>(s => s.User)
                .WithMany(g => g.Messages)
                //.HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
               .HasOne<Chat>(s => s.Chat)
               .WithMany(g => g.Messages)
               //.HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Chat>()
               .HasOne<ApplicationUser>(s => s.SenderUser)
               .WithMany(g => g.SenderChats);
            //.HasForeignKey(s => s.UserId)

            modelBuilder.Entity<Chat>()
               .HasOne<ApplicationUser>(s => s.RecipientUser)
               .WithMany(g => g.RecipientChat);
            //.HasForeignKey(s => s.UserId)
        }
    }
}
