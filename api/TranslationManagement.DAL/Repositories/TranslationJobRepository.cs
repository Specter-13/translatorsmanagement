using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.DAL.Repositories
{
    public class TranslationJobRepository
    {

        private readonly AppDbContext _appDbContext;

        // here should be all crud operations
        public TranslationJobRepository(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;

        }

        public IList<TranslationJob> GetTranslators()
        {
            return _appDbContext.TranslationJobs.ToList();
        }

        public TranslationJob GetById(int id)
        {
            return _appDbContext.TranslationJobs.FirstOrDefault(t => t.Id == id);
        }

        public TranslationJob Create(TranslationJob job)
        {
            //if (job != null)
            //{
            //    var returnedTranslator = _appDbContext.Translators.FirstOrDefault(x => x.Name == translator.Name);
            //    if (returnedTranslator != null)
            //    {
            //        return null;
            //    }
            //}
            job.Status = JobStatus.New;
            _appDbContext.TranslationJobs.Add(job);
            _appDbContext.SaveChanges();
            return job;
        }
    }
}
