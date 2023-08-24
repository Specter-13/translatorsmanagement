using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Client;
using TranslationManagement.Web.Facade;

namespace TranslationManagement.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });
            builder.Services.AddScoped<ITranslationJobClient, TranslationJobClient>();
            builder.Services.AddScoped<ITranslatorManagementClient, TranslatorManagementClient>();

            builder.Services.AddScoped<TranslatorManagementFacade>();
            await builder.Build().RunAsync();
        }
    }
}
