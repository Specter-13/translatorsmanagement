using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TranslationManagement.Client;
using TranslationManagement.Web;
using TranslationManagement.Web.Facade;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5000") });
builder.Services.AddScoped<ITranslationJobClient, TranslationJobClient>();
builder.Services.AddScoped<ITranslatorManagementClient, TranslatorManagementClient>();

builder.Services.AddScoped<TranslatorManagementFacade>();

await builder.Build().RunAsync();
