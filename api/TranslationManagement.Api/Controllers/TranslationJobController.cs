using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.JobFileReader;
using TranslationManagement.DAL;
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

        [HttpGet]
        public IActionResult GetJobs()
        {
            return Ok(_translationJobRepository.GetJobs());
        }

        

        [HttpPost]
        public IActionResult CreateJob(TranslationJob job)
        {

            var createdJob = _translationJobRepository.Create(job);
            if (createdJob != null)
            {
               SendNotification(createdJob.Id);
            }

            return Ok(createdJob);
        }

        [HttpPost]
        public IActionResult CreateJobWithFile(IFormFile file, string customer)
        {
            var readJob = _jobReader.ExecuteRead(file, customer);
            if (readJob == null) 
            {
                BadRequest("Cannot read file, file may not be supported");
            }
            var createdJob = _translationJobRepository.Create(readJob);
            if (createdJob != null)
            {
                SendNotification(createdJob.Id);
            }

            return Ok(createdJob);
        }

        [HttpPut]
        public IActionResult UpdateJobStatus(int jobId, int translatorId, JobStatus newStatus)
        {
            _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);

            // check for invalid input
            //if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
            //{
            //    return "invalid status";
            //}

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
            while (!_notificationService.SendNotification("Job created: " + jobId).Result)
            {
            }

            _logger.LogInformation("New job notification sent");
        }

      

    }
}