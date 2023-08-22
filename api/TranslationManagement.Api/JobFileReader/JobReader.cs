using Microsoft.AspNetCore.Http;
using System.Linq;
using TranslationManagement.Api.JobFileReader.ConcreteReaders;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.Api.JobFileReader
{
    public class JobReader
    {
        private IJobReader _reader;

        public void SetReader(IJobReader reader)
        {
            _reader = reader;
        }

        public TranslationJob ExecuteRead(IFormFile file, string customer)
        {

           var isSuccess = SetReaderByFileExtension(file);
           if (!isSuccess) 
           {
                return null;
           }


           return _reader.ReadContent(file, customer);
        }


        private bool SetReaderByFileExtension(IFormFile file) 
        {
            var extension = file.FileName.Split('.').Last();
            

            switch (extension) 
            {
                case "xml" :
                    {
                        this.SetReader(new JobXmlFileReader());
                        return true;
                    }
                case "txt":
                    {
                        this.SetReader(new JobTextFileReader());
                        return true;
                    }
                default: 
                    {
                        return false;
                    }
            }
        }

    }
}
