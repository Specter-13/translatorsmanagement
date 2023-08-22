using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.DAL.Enums;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.DAL.Helpers
{
    public static class JobHelper
    {
        const double PricePerCharacter = 0.01;
        public static void SetPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * PricePerCharacter;
        }

        public static bool CheckForStatusValidity(JobStatus oldStatus, JobStatus newStatus) =>
          (oldStatus == JobStatus.New && newStatus == JobStatus.Completed) ||
              oldStatus == JobStatus.Completed || newStatus == JobStatus.New;



    }
}
