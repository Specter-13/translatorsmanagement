using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.DAL.Repositories
{
    public class TranslatorRepository
    {
        private readonly AppDbContext _appDbContext;

        // here should be all crud operations
        public TranslatorRepository(AppDbContext appDbContext)
        {

            _appDbContext = appDbContext;

        }

        public IList<Translator> GetTranslators()
        {
            return _appDbContext.Translators.ToList();
        }

        public Translator GetById(int id)
        {
            return _appDbContext.Translators.FirstOrDefault(t => t.Id == id);
        }

        public IList<Translator> GetTranslatorsByName(string name)
        {
            return _appDbContext.Translators.Where(t => t.Name == name).ToList();
        }


        public Translator AddTranslator(Translator translator)
        {
            //if (translator != null)
            //{
            //    var returnedTranslator = _appDbContext.Translators.FirstOrDefault(x => x.Name == translator.Name);
            //    if (returnedTranslator != null)
            //    {
            //        return null;
            //    }
            //}

            _appDbContext.Translators.Add(translator);
            _appDbContext.SaveChanges();
            return translator;
        }

        public Translator UpdateTranslator(Translator translator)
        {
            _appDbContext.Translators.Update(translator);
            _appDbContext.SaveChanges();
            return translator;
        }

        public void Delete(int id)
        {

            var entity = _appDbContext.Translators.First(t => t.Id == id);
            _appDbContext.Remove(entity);
            _appDbContext.SaveChanges();
        }
    }
}
