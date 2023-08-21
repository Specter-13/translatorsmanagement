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
using TranslationManagement.DAL;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Models;
using TranslationManagement.DAL.Repositories;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/jobs/[action]")]
    public class TranslationJobController : ControllerBase
    {
      
        static class JobStatuses
        {
            internal static readonly string New = "New";
            internal static readonly string Inprogress = "InProgress";
            internal static readonly string Completed = "Completed";
        }

        private AppDbContext _context;
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly TranslationJobRepository _translationJobRepository;
        public TranslationJobController(TranslationJobRepository translationJobRepository, ILogger<TranslatorManagementController> logger)
        {
            _translationJobRepository = translationJobRepository;
            _logger = logger;
        }

        [HttpGet]
        public TranslationJob[] GetJobs()
        {
            return _context.TranslationJobs.ToArray();
        }

        const double PricePerCharacter = 0.01;
        private void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }

        [HttpPost]
        public bool CreateJob(TranslationJob job)
        {
            job.Status = JobStatus.New;
            SetPrice(job);
            _context.TranslationJobs.Add(job);
            bool success = _context.SaveChanges() > 0;
            if (success)
            {
                var notificationSvc = new UnreliableNotificationService();
                while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
                {
                }

                _logger.LogInformation("New job notification sent");
            }

            return success;
        }

        [HttpPost]
        public bool CreateJobWithFile(IFormFile file, string customer)
        {
            var reader = new StreamReader(file.OpenReadStream());
            string content;

            if (file.FileName.EndsWith(".txt"))
            {
                content = reader.ReadToEnd();
            }
            else if (file.FileName.EndsWith(".xml"))
            {
                var xdoc = XDocument.Parse(reader.ReadToEnd());
                content = xdoc.Root.Element("Content").Value;
                customer = xdoc.Root.Element("Customer").Value.Trim();
            }
            else
            {
                throw new NotSupportedException("unsupported file");
            }

            var newJob = new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer,
            };

            SetPrice(newJob);

            return CreateJob(newJob);
        }

        [HttpPost]
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

            bool isInvalidStatusChange = CheckForStatusValidity(job.Status, newStatus);
            if (isInvalidStatusChange)
            {
                return BadRequest("Invalid status change");
            }

            job.Status = newStatus;
            _context.SaveChanges();
            return Ok();
        }



        private bool CheckForStatusValidity(JobStatus oldStatus, JobStatus newStatus) =>
            (oldStatus == JobStatus.New && newStatus == JobStatus.Completed) ||
                oldStatus == JobStatus.Completed || newStatus == JobStatus.New;

    }
}