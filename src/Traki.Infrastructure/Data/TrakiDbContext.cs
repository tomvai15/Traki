using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Section.Items;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
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

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.Protocols)
                .WithOne(p => p.Product);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.Drawings)
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

            modelBuilder.Entity<CompanyEntity>()
                .HasMany(p => p.Projects)
                .WithOne(p => p.Company);

            modelBuilder.Entity<DrawingEntity>()
                .HasMany(p => p.Defects)
                .WithOne(p => p.Drawing);

            modelBuilder.Entity<DefectEntity>()
                .HasMany(p => p.DefectComments)
                .WithOne(p => p.Defect)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DefectEntity>()
                .HasMany(p => p.StatusChanges)
                .WithOne(p => p.Defect)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserEntity>()
                .HasMany(p => p.Defects)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserEntity>()
                .HasMany(p => p.Projects)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<UserEntity>()
                .HasMany(p => p.Products)
                .WithOne(p => p.User)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserEntity>()
                .HasMany(p => p.DefectComments)
                .WithOne(p => p.Author)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserEntity>()
                .HasMany(p => p.StatusChanges)
                .WithOne(p => p.Author)
                .OnDelete(DeleteBehavior.NoAction);
        }


        public virtual DbSet<DrawingEntity> Drawings { get; set; }
        public virtual DbSet<DefectEntity> Defects { get; set; }
        public virtual DbSet<DefectComment> DefectComments { get; set; }
        public virtual DbSet<StatusChangeEntity> StatusChanges { get; set; }

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
