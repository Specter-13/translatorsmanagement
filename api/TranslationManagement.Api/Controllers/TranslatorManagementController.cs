using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
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

        [HttpGet("All")]
        [SwaggerResponse(typeof(ICollection<Translator>))]
        public ActionResult<ICollection<Translator>> GetTranslators()
        {
            return Ok(_translatorRepository.GetTranslators());
        }

        [HttpGet("GetByName")]
        public IActionResult GetTranslatorsByName(string name)
        {
            return Ok(_translatorRepository.GetTranslatorsByName(name));
        }

        [HttpPost("Create")]
        [SwaggerResponse(typeof(Translator))]
        public IActionResult AddTranslator([FromBody] Translator translator)
        {
            var addedTranslator = _translatorRepository.AddTranslator(translator);
            if(addedTranslator == null) 
            {
                return BadRequest();
            }

            return Ok(addedTranslator);
        }
        
        [HttpPut("UpdateStatus")]
        public IActionResult UpdateTranslatorStatus(int id, TranslatorStatus status)
        {
            _logger.LogInformation("User status update request: " + status + " for user " + id.ToString());


            var translator = _translatorRepository.GetById(id);
            if(translator == null) 
            {
                return BadRequest();
            }

            translator.Status = status;

            _translatorRepository.UpdateTranslator(translator);

            return Ok(translator);
        }
    }
}