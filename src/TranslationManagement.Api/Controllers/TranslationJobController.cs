using System;
using System.Collections.Generic;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.JobFileReader;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Helpers;
using TranslationManagement.DAL.Models;
using TranslationManagement.DAL.Repositories;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
  
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly TranslationJobRepository _translationJobRepository;
        private readonly JobReader _jobReader = new JobReader();
        private readonly UnreliableNotificationService _notificationService = new UnreliableNotificationService();
        public TranslationJobController(TranslationJobRepository translationJobRepository, ILogger<TranslatorManagementController> logger)
        {
            _translationJobRepository = translationJobRepository;
            _logger = logger;
        }


        /// <summary>
        /// Here I would document each methods, also other stuff in solution
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        [SwaggerResponse(typeof(ICollection<TranslationJob>))]
        public ActionResult<ICollection<TranslationJob>> GetJobs()
        {
            return Ok(_translationJobRepository.GetJobs());
        }

        

        [HttpPost("Create")]
        public IActionResult CreateJob([FromBody] TranslationJob job)
        {

            var createdJob = _translationJobRepository.Create(job);
            if (createdJob != null)
            {
               SendNotification(createdJob.Id);
            }

            return Ok();
        }

        [HttpPost("CreateWithFile")]
        public IActionResult CreateJobWithFile(IFormFile file, string customer)
        {
            var readJob = _jobReader.Execute(file, customer);
            if (readJob == null) 
            {
                BadRequest("Cannot read file, file may not be supported");
            }
            var createdJob = _translationJobRepository.Create(readJob);
            if (createdJob != null)
            {
                SendNotification(createdJob.Id);
            }

            return Ok();
        }

        [HttpPut("UpdateStatus")]
        public IActionResult UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
        {
            _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
       
            // if input enum is not defined, return bad request
            if(!Enum.IsDefined(typeof(JobStatus), newStatus))
            {
                return BadRequest("Undefined status");
            }

            var job = _translationJobRepository.GetById(jobId);
            if(job == null) 
            {
                return NotFound("Job does not exist");
            }

            bool isInvalidStatusChange = JobHelper.CheckForStatusValidity(job.Status, newStatus);
            if (isInvalidStatusChange)
            {
                return BadRequest("Invalid status change");
            }

            job.Status = newStatus;
            _translationJobRepository.Update(job);
  
            return Ok();
        }

        private void SendNotification(int jobId)
        {
            try
            {
                while (!_notificationService.SendNotification("Job created: " + jobId).Result)
                {
                }

                _logger.LogInformation("New job notification sent");

            }
           
            catch (Exception e)
            {
                // swallow, or we should try again to send notification, I don't know
            
            }
        }

      

    }
}