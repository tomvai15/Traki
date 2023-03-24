﻿using Microsoft.Extensions.DependencyInjection;

namespace Traki.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void AddInitialData(this IServiceProvider serviceProvider, bool isDevelopment)
        {
            var dbContext = serviceProvider.GetRequiredService<TrakiDbContext>();

            if (isDevelopment)
            {
                // this can be done only locally (already deleted database in cloud once)
                dbContext.Database.EnsureDeleted();
            }

            bool wasCreated = dbContext.Database.EnsureCreated();

            dbContext.AddCompanies();
            dbContext.AddProjects();
            dbContext.AddProducts();
            dbContext.AddTemplates();
            dbContext.AddQuestions();
            dbContext.AddChecklists();
            dbContext.AddChecklistQuestions();
            dbContext.AddUsers();
        }

        public static TrakiDbContext AddCompanies(this TrakiDbContext dbContext)
        {
            dbContext.Companies.AddRange(ExampleData.Companies);
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
            dbContext.Questions.AddRange(ExampleData.Questions);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddChecklists(this TrakiDbContext dbContext)
        {
            dbContext.Checklists.AddRange(ExampleData.Checklists);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddChecklistQuestions(this TrakiDbContext dbContext)
        {
            dbContext.CheckListQuestions.AddRange(ExampleData.CheckListQuestions);
            dbContext.SaveChanges();

            return dbContext;
        }

        public static TrakiDbContext AddUsers(this TrakiDbContext dbContext)
        {
            dbContext.Users.AddRange(ExampleData.Users);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}