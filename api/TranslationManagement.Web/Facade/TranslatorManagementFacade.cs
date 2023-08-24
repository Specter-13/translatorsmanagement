using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TranslationManagement.Client;

namespace TranslationManagement.Web.Facade
{
    public class TranslatorManagementFacade
    {
        private readonly ITranslatorManagementClient _translatorManagementClient;
        public TranslatorManagementFacade(ITranslatorManagementClient translatorManagementClient)
        {
            _translatorManagementClient = translatorManagementClient;
        }


        public async Task<ICollection<Translator>> AllTranslators()
        {
            try
            {
                var result = await _translatorManagementClient.All2Async();
                if (result.StatusCode == (int)HttpStatusCode.OK)
                {
                    return result.Result;
                }
            }
            catch (Exception e)
            {
                return new List<Translator>();
            }


            return new List<Translator>();
        }


        public async Task Create(Translator translator)
        {
            var result = await _translatorManagementClient.Create2Async(translator);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                return;
            }
        
        }


        public async Task UpdateStatus(int id, TranslatorStatus translatorStatus)
        {
            var result = await _translatorManagementClient.UpdateStatus2Async(id, translatorStatus);

            if (result.StatusCode == (int)HttpStatusCode.OK)
            {
                return;
            }

        }
    }
}
