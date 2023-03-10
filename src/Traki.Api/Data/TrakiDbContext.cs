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

            modelBuilder.Entity<TemplateEntity>()
                .HasMany(p => p.Questions)
                .WithOne(p => p.Template);
        }


        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProjectEntity> Projects { get; set; }
        public virtual DbSet<TemplateEntity> Templates { get; set; }
        public virtual DbSet<QuestionEntity> Questions { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
