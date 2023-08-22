using Microsoft.AspNetCore.Http;
using TranslationManagement.DAL.Models;

namespace TranslationManagement.Api.JobFileReader
{
    public interface IJobReader
    {
        TranslationJob ReadContent(IFormFile file, string customer);
    }
}
