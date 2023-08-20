using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.DAL;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Models;
using TranslationManagement.DAL.Repositories;

namespace TranslationManagement.Api.Controlers
{
    [ApiController]
    [Route("api/TranslatorsManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {
      
        public static readonly string[] TranslatorStatuses = { "Applicant", "Certified", "Deleted" };

        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly TranslatorRepository _translatorRepository;

        public TranslatorManagementController(TranslatorRepository translatorRepository, ILogger<TranslatorManagementController> logger)
        {
            _translatorRepository = translatorRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTranslators()
        {
            return Ok(_translatorRepository.GetTranslators());
        }

        [HttpGet]
        public IActionResult GetTranslatorsByName(string name)
        {
            return Ok(_translatorRepository.GetTranslatorsByName(name));
        }

        [HttpPost]
        public IActionResult AddTranslator(Translator translator)
        {
            var addedTranslator = _translatorRepository.AddTranslator(translator);
            if(addedTranslator == null) 
            {
                return BadRequest();
            }

            return Ok(addedTranslator);
        }
        
        [HttpPut]
        public IActionResult UpdateTranslatorStatus(int id, TranslatorStatus status)
        {
            _logger.LogInformation("User status update request: " + status + " for user " + id.ToString());


            var job = _translatorRepository.GetById(id);
            if(job == null) 
            {
                return BadRequest();
            }

            job.Status = status;

            _translatorRepository.UpdateTranslator(job);

            return Ok(job);
        }
    }
}