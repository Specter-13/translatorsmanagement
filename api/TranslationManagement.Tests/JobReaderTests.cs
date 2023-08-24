using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Api.JobFileReader;
using Xunit;

namespace TranslationManagement.Tests
{
    public class JobReaderTests
    {
        private JobReader _jobReader;
        public JobReaderTests()
        {
            _jobReader = new JobReader();
        }


        [Fact]
        public void job_reader_xml_read_successfull()
        {
            //Arrange
            //Setup mock file using a memory stream
            var content = File.ReadAllText("TestFiles/job.xml");
            var fileName = "test.xml";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "test", fileName);

            //Act
            var readJob = _jobReader.ExecuteRead(file, "testCustomer");

            //Assert
            Assert.NotNull(readJob);
            Assert.Equal("testCustomer", readJob.CustomerName);

        }
    }
}
