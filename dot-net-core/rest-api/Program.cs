using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace rest2
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
      .ConfigureAppConfiguration((builderContext, config) =>
        {
          var env = builderContext.HostingEnvironment;

          //Environment name is in System Variable ASPNETCORE_ENVIRONMENT=Development
          string envName = env.EnvironmentName;

					Console.WriteLine("Runs in {0}", envName);
          config.AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{envName}.json", false, true);

        })
        .UseStartup<Startup>();

  }
}
