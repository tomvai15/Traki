using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Api.Mapping;
using Traki.Domain.Models.Section;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using static Traki.UnitTests.Fakes.Dummy;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    public class TableRepositoryTests: IClassFixture<TrakiDbFixture>
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper mapper;

        public TableRepositoryTests(TrakiDbFixture trakiDbFixture) 
        {
            IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToDomainModelMappingProfile());
                cfg.AddProfile(new DomainToContractMappingProfile());
            });

            mapper = new Mapper(configuration);
            _trakiDbFixture = trakiDbFixture;
        }
        /*

        [Fact]
        public async void CreateTable_CreatesTable()
        {
            var table = Any<Table>();


            using (var context = new CinemaDbContext(cinemaDbfixture.Options))
            {
                UsersHandler usersHandler = new UsersHandler(context);

                await usersHandler.Get();

                var a = context.Users.ToList();
                return;
            }
        }
        */

        /*
        public async Task DeleteTable(int tableId)
        {
            var table = await _context.Tables.Where(x => x.Id == tableId).FirstOrDefaultAsync();

            if (table == null)
            {
                return;
            }
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }

        public async Task<Table> GetSectionTable(int sectionId)
        {
            var tableEntity = await _context.Tables.Where(x => x.SectionId == sectionId)
                .Include(x => x.TableRows).ThenInclude(x => x.RowColumns)
                .FirstOrDefaultAsync();
            return _mapper.Map<Table>(tableEntity);
        }*/
    }
}
