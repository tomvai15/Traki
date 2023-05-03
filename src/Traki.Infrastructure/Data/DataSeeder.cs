using DocuSign.eSign.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Traki.Infrastructure.Entities;

namespace Traki.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async void AddInitialData(this IServiceProvider serviceProvider, bool isDevelopment)
        {
            var dbContext = serviceProvider.GetRequiredService<TrakiDbContext>();

            //return;
            var users = ExampleData.Users.ToList();
            /*
            try
            {
                users = await dbContext.Users.ToListAsync();

                foreach (var user in users)
                {
                    user.Id = 0;
                }

                if (users.Count == 0)
                {
                    users = ExampleData.Users.ToList();
                }
            } catch (Exception ex)
            {

            }
            */

            if (isDevelopment)
            {
                // this can be done only locally (already deleted database in cloud once)
                dbContext.Database.EnsureDeleted();
            }

            bool wasCreated = dbContext.Database.EnsureCreated();

            dbContext.AddUsers(users);

            dbContext.AddCompanies();
            dbContext.AddProjects();
            dbContext.AddProducts();
            dbContext.AddTemplates();
            dbContext.AddQuestions();
            dbContext.AddChecklists();
            dbContext.AddChecklistQuestions();

            dbContext.AddProtocols();
            dbContext.AddSections();
            dbContext.AddCheclists();
            dbContext.AddItems();
            dbContext.AddNewQuestions();
            dbContext.AddTextInputs();
            dbContext.AddMultipleChoices();

            dbContext.AddDrawings();
            dbContext.AddDefects();
        }

        public static TrakiDbContext AddCompanies(this TrakiDbContext dbContext)
        {
            dbContext.Companies.AddRange(ExampleData.Companies);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddDrawings(this TrakiDbContext dbContext)
        {
            dbContext.Drawings.AddRange(ExampleData.Drawings);
            dbContext.SaveChanges();

            return dbContext;
        }
        public static TrakiDbContext AddDefects(this TrakiDbContext dbContext)
        {
            dbContext.Defects.AddRange(ExampleData.Defects);
            dbContext.SaveChanges();

            return dbContext;
        }
        public static TrakiDbContext AddProtocols(this TrakiDbContext dbContext)
        {
            dbContext.Protocols.AddRange(ExampleData.Protocols);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddSections(this TrakiDbContext dbContext)
        {
            dbContext.Sections.AddRange(ExampleData.Sections);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddCheclists(this TrakiDbContext dbContext)
        {
            dbContext.Checklists.AddRange(ExampleData.NewChecklists);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddItems(this TrakiDbContext dbContext)
        {
            dbContext.Items.AddRange(ExampleData.Items);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddNewQuestions(this TrakiDbContext dbContext)
        {
            dbContext.Questions.AddRange(ExampleData.NewQuestions);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddTextInputs(this TrakiDbContext dbContext)
        {
            dbContext.TextInputs.AddRange(ExampleData.TextInputs);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddMultipleChoices(this TrakiDbContext dbContext)
        {
            dbContext.MultipleChoices.AddRange(ExampleData.MultipleChoices);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddProjects(this TrakiDbContext dbContext)
        {
            dbContext.Projects.AddRange(ExampleData.Projects);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddProducts(this TrakiDbContext dbContext)
        {
            dbContext.Products.AddRange(ExampleData.Products);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddTemplates(this TrakiDbContext dbContext)
        {
            dbContext.Templates.AddRange(ExampleData.Templates);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddQuestions(this TrakiDbContext dbContext)
        {
           // dbContext.OldQuestions.AddRange(ExampleData.Questions);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddChecklists(this TrakiDbContext dbContext)
        {
            dbContext.OldChecklists.AddRange(ExampleData.Checklists);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddChecklistQuestions(this TrakiDbContext dbContext)
        {
            dbContext.CheckListQuestions.AddRange(ExampleData.CheckListQuestions);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddUsers(this TrakiDbContext dbContext, List<UserEntity> users)
        {
            dbContext.Users.AddRange(users);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
