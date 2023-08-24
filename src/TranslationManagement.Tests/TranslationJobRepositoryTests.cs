using System;
using System.Linq;
using TranslationManagement.DAL;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Helpers;
using TranslationManagement.DAL.Models;
using TranslationManagement.DAL.Repositories;
using TranslationManagement.Tests.Setup;
using Xunit;

namespace TranslationManagement.Tests
{
    public class TranslationJobRepositoryTests : IDisposable
    {
        private readonly AppDbContext _testContext;
        private readonly TranslationJobRepository _translationJobRepository;

        public TranslationJobRepositoryTests()
        {
            var setup = new TestDBInMemoryContextSetup("testDb");
            _testContext = setup.CreateInMemoryDbContext();
            setup.PrepareDatabase();
            _translationJobRepository = new TranslationJobRepository(_testContext);
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
               OriginalContent = "Test",
               TranslatedContent = "",
               Price = 10
  
            };
            //Act
            var createdJob = _translationJobRepository.Create(job);
            var isCreated = _translationJobRepository.GetById(job.Id);

            //Assert
            Assert.NotNull(createdJob);
            Assert.NotNull(isCreated);
            Assert.Equal(JobStatus.New, createdJob.Status);
            
        }
    }
}
