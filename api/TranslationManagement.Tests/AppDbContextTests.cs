using System;
using System.Linq;
using TranslationManagement.DAL;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Helpers;
using TranslationManagement.DAL.Models;
using TranslationManagement.Tests.Setup;
using Xunit;

namespace TranslationManagement.Tests
{
    public class AppDbContextTests : IDisposable
    {
        private readonly AppDbContext _testContext;
        public AppDbContextTests()
        {
            var setup = new TestDBInMemoryContextSetup("testDb");
            _testContext = setup.CreateInMemoryDbContext();
            
            setup.PrepareDatabase();
        }

        public void Dispose() => _testContext.Dispose();

        [Fact]
        public void job_created_test_successfull()
        {
            //Arrange

            var job = new TranslationJob()
            {
               Id = 1,
               CustomerName = "Test",
               Status = JobStatus.New,
               OriginalContent = "Test",
               TranslatedContent = "",
               Price = 10
  
            };
            //Act
            var createdJob = _testContext.TranslationJobs.Add(job);
            _testContext.SaveChanges();
            var isCreated = _testContext.TranslationJobs.FirstOrDefault(x => x.Id == job.Id);

            //Assert
            Assert.NotNull(createdJob);
            Assert.NotNull(isCreated);

            

            
        }
    }
}
