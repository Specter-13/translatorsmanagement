using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection.PortableExecutable;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.Api.JobFileReader.ConcreteReaders
{
    public class JobTextFileReader : IJobReader
    {
        public TranslationJob ReadContent(IFormFile file, string customer)
        {
            var reader = new StreamReader(file.OpenReadStream());
            
            var content = reader.ReadToEnd();
            reader.Close();


            return new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer
            };
        }
    }
}
