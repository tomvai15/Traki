﻿using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class StatusChangeRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public StatusChangeRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task CreateStatusChange_CreatesStatusChange()
        {
            var statusChange = new StatusChange
            {
                From = DefectStatus.NotFixed,
                To = DefectStatus.Unfixable,
                DefectId = 1,
                AuthorId = 1
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new StatusChangeRepository(context, _mapper);

            await repository.CreateStatusChange(statusChange);

            var statusChangeFromDb = context.StatusChanges.FirstOrDefault();

            statusChangeFromDb.From.Should().Be(statusChange.From);
            statusChangeFromDb.To.Should().Be(statusChange.To);
            statusChangeFromDb.DefectId.Should().Be(statusChange.AuthorId);
            statusChangeFromDb.AuthorId.Should().Be(statusChange.AuthorId);
            statusChangeFromDb.Date.Should().NotBeNullOrEmpty();
        }
    }
}
