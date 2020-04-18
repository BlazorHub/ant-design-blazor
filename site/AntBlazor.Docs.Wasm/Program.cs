using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Autofac;

namespace AntBlazor.Docs.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            //add Autofac
            var containerBuilder = new ContainerBuilder();
            builder.ConfigureContainer(new AbpAutofacServiceProviderFactory(containerBuilder));
            builder.Services.AddObjectAccessor(containerBuilder);

            builder.Services.AddSingleton(new HttpClient
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            var configurationBuilder = new ConfigurationBuilder();
            
            //var stream = Assembly.GetAssembly(typeof(Program)).GetManifestResourceStream("appsetting.json");
            //configurationBuilder.AddJsonStream(stream);
            
            //builder.Configuration.AddJsonStream(stream);
            //Need to register IConfiguration manually
            builder.Services.AddSingleton(typeof(IConfiguration), configurationBuilder.Build());
         
            builder.Services.AddApplication<AntBlazorDocsWasmModule>();
 
            await builder.Build().RunAsync();
        }
    }
}