using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Extensions.Configuration.ConfigServer;

namespace RestApiShowcase
{
  public class Program
  {
    public static void Main(string[] args)
    {
			var builder = WebHost.CreateDefaultBuilder(args);
			builder = builder.ConfigureAppConfiguration(configureMe);
			builder = builder.UseStartup<Startup>();
			var host = builder.Build();
			host.Run();
    }

		private static void configureMe(WebHostBuilderContext hostingContext, IConfigurationBuilder config) {
			var env = hostingContext.HostingEnvironment;
			LogLevel level = LogLevel.Information;
			if (env.EnvironmentName == "Development") level = LogLevel.Trace;
			config.AddConfigServer(new LoggerFactory().AddConsole(level));
		}

  }
}
