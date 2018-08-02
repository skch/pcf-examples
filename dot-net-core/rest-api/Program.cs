using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace rest2
{
  public class Program
  {
    public static void Main(string[] args)
    {
			var builder = WebHost.CreateDefaultBuilder(args);
			builder = builder.ConfigureAppConfiguration(conrigureBld);
			//builder = configureLogging(builder);
			//builder = builder.UseIISIntegration();
			builder = builder.UseStartup<Startup>();
			var host = builder.Build();
			host.Run();
    }

		private static void conrigureBld(WebHostBuilderContext hostingContext, IConfigurationBuilder config) {
			var env = hostingContext.HostingEnvironment;
			config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

			//config.AddConfigServer(env);
			config.AddEnvironmentVariables();
		}

		public static IWebHostBuilder configureLogging(IWebHostBuilder builder) {
			return builder.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddDebug();
					logging.AddConsole();
				});
		}

  }
}
