using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.DAL;

namespace TranslationManagement.Tests.Setup
{
    internal class TestDBInMemoryContextSetup
    {
        private string _testDbName;
        public TestDBInMemoryContextSetup(string testDbName)
        {
            _testDbName = testDbName;
        }


        public AppDbContext CreateInMemoryDbContext()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            dbContextOptionsBuilder.UseInMemoryDatabase(_testDbName);
            dbContextOptionsBuilder.EnableSensitiveDataLogging();
            return new AppDbContext(dbContextOptionsBuilder.Options);
        }


        public void PrepareDatabase()
        {
            using var dbx = CreateInMemoryDbContext();
            dbx.Database.EnsureCreated();
        }

        public void TearDownDatabase()
        {
            using var dbx = CreateInMemoryDbContext();
            dbx.Database.EnsureDeleted();
        }

        public void Dispose()
        {
            TearDownDatabase();
        }
    }
}
