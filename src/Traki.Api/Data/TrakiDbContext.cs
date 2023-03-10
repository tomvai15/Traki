using Microsoft.EntityFrameworkCore;
using Traki.Api.Entities;
using Traki.Api.Models;

namespace Traki.Api.Data
{
    public class TrakiDbContext : DbContext
    {
        public TrakiDbContext(DbContextOptions<TrakiDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectEntity>()
                .HasMany(p => p.Products)
                .WithOne(p => p.Project);

            modelBuilder.Entity<ProjectEntity>()
                .HasMany(p => p.Templates)
                .WithOne(navigationExpression: p => p.Project);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.CheckLists)
                .WithOne(p => p.Product);

            modelBuilder.Entity<TemplateEntity>()
                .HasMany(p => p.Questions)
                .WithOne(p => p.Template);

            modelBuilder.Entity<ChecklistEntity>()
                .HasMany(p => p.CheckListQuestions)
                .WithOne(p => p.Checklist);
        }


        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProjectEntity> Projects { get; set; }
        public virtual DbSet<TemplateEntity> Templates { get; set; }
        public virtual DbSet<QuestionEntity> Questions { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
