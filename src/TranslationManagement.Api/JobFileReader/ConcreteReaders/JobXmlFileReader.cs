using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.Api.JobFileReader.ConcreteReaders
{
    public class JobXmlFileReader : IJobReader
    {
       
        public TranslationJob ReadContent(IFormFile file, string customer)
        {
            
            var reader = new StreamReader(file.OpenReadStream());
            var xdoc = XDocument.Parse(reader.ReadToEnd());
            var content = xdoc.Root.Element("Content").Value;

            if (string.IsNullOrEmpty(customer))
            {
                customer = xdoc.Root.Element("Customer").Value.Trim();
            }
       

            reader.Close();
           

            return new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer,
            };
        }
    }
}
