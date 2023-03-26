using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Section.Items;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Data
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
                .WithOne(p => p.Project);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.CheckLists)
                .WithOne(p => p.Product);

            modelBuilder.Entity<TemplateEntity>()
                .HasMany(p => p.Questions)
                .WithOne(p => p.Template);

            modelBuilder.Entity<OldChecklistEntity>()
                .HasMany(p => p.ChecklistQuestions)
                .WithOne(p => p.Checklist);

            modelBuilder.Entity<SectionEntity>()
                .HasOne(p => p.Checklist).WithOne(p => p.Section)
                .HasForeignKey<ChecklistEntity>(e => e.SectionId);

            modelBuilder.Entity<SectionEntity>()
                .HasOne(p => p.Table).WithOne(p => p.Section)
                .HasForeignKey<TableEntity>(e => e.SectionId);

            modelBuilder.Entity<ChecklistEntity>()
                .HasMany(p => p.Items)
                .WithOne(p => p.Checklist);

            modelBuilder.Entity<MultipleChoiceEntity>()
                .HasMany(p => p.Options)
                .WithOne(p => p.MultipleChoice)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(p => p.TextInput).WithOne(p => p.Item)
                .HasForeignKey<TextInputEntity>(e => e.ItemId);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(p => p.MultipleChoice).WithOne(p => p.Item)
                .HasForeignKey<MultipleChoiceEntity>(e => e.ItemId);

            modelBuilder.Entity<ItemEntity>()
                .HasOne(p => p.Question).WithOne(p => p.Item)
                .HasForeignKey<QuestionEntity>(e => e.ItemId);

            modelBuilder.Entity<ProtocolEntity>()
                .HasMany(p => p.Sections)
                .WithOne(p => p.Protocol);
        }

        public virtual DbSet<ProtocolEntity> Protocols { get; set; }

        public virtual DbSet<SectionEntity> Sections { get; set; }
        public virtual DbSet<ChecklistEntity> Checklists { get; set; }
        public virtual DbSet<TableEntity> Tables { get; set; }

        public virtual DbSet<ItemEntity> Items { get; set; }
        public virtual DbSet<MultipleChoiceEntity> MultipleChoices { get; set; }
        public virtual DbSet<OptionEntity> Options { get; set; }
        public virtual DbSet<QuestionEntity> Questions { get; set; }
        public virtual DbSet<TextInputEntity> TextInputs { get; set; }


        public virtual DbSet<CompanyEntity> Companies { get; set; }
        public virtual DbSet<OldChecklistEntity> OldChecklists { get; set; }
        public virtual DbSet<ChecklistQuestionEntity> CheckListQuestions { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<ProjectEntity> Projects { get; set; }
        public virtual DbSet<TemplateEntity> Templates { get; set; }
        public virtual DbSet<QuestionEntity> OldQuestions { get; set; }
        public virtual DbSet<UserEntity> Users { get; set; }
    }
}
