using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Helpers;
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

        public IList<TranslationJob> GetJobs()
        {
            return _appDbContext.TranslationJobs.ToList();
        }

        public TranslationJob GetById(int id)
        {
            return _appDbContext.TranslationJobs.FirstOrDefault(t => t.Id == id);
        }

        public TranslationJob Create(TranslationJob job)
        {
            JobHelper.SetPrice(job);
            job.Status = JobStatus.New;
            _appDbContext.TranslationJobs.Add(job);
            _appDbContext.SaveChanges();
            return job;
        }

        public TranslationJob Update(TranslationJob job)
        {
            _appDbContext.TranslationJobs.Update(job);
            _appDbContext.SaveChanges();
            return job;
        }

        public void Delete(int id)
        {

            var entity = _appDbContext.TranslationJobs.First(t => t.Id == id);
            _appDbContext.Remove(entity);
            _appDbContext.SaveChanges();
        }
    }
}
