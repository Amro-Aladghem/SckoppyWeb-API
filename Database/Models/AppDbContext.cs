using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class AppDbContext :DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public AppDbContext()
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder.UseSqlServer(clsConnection.ConnectionString));
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Token> Tokens { get; set; }    
        public DbSet<LoginHistory> LoginHistories { get; set; } 
        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            ConfigureUserEntity(modelBuilder);
            ConfigurePostEntity(modelBuilder);
            

            modelBuilder.Entity<Comment>().Property(P => P.CreateAt)
                                          .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Token>().Property(P => P.CreationDateTime)
                                        .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Token>().Property(P => P.IsActive)
                                        .HasDefaultValue(true);

            modelBuilder.Entity<LoginHistory>().Property(P => P.LoginDateTime)
                                               .HasDefaultValueSql("GetDate()");

            modelBuilder.Entity<Tag>().HasData(
                new Tag() { TagId = 1, Name = "Policy", Description = "About Policy" },
                new Tag() { TagId = 2, Name = "economy", Description = "About economy" },
                new Tag() { TagId = 3, Name = "History", Description = "About History" }
                );


            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))

            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


        }

        private void ConfigureUserEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(P => P.CommentsCount)
                                       .HasDefaultValue(0);

            modelBuilder.Entity<User>().Property(P => P.LikesCount)
                                       .HasDefaultValue(0);

            modelBuilder.Entity<User>().Property(P => P.PostCount)
                                       .HasDefaultValue(0);

            modelBuilder.Entity<User>().Property(P => P.CreatedDateTime)
                                       .HasDefaultValueSql("GETDATE()");
        }

        private void ConfigurePostEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Property(P => P.CommentCount)
                                       .HasDefaultValue(0);

            modelBuilder.Entity<Post>().Property(P => P.LikesCount)
                                       .HasDefaultValue(0);

            modelBuilder.Entity<Post>().Property(P => P.IsUpdated)
                                       .HasDefaultValue(false);

            modelBuilder.Entity<Post>().Property(P => P.CreatedDateTime)
                                       .HasDefaultValueSql("GETDATE()");
        }

    }
}
