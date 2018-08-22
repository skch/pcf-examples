using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using RestApiShowcase.Models;

namespace RestApiShowcase
{
	public class Startup
	{
		private readonly ILogger<Startup> _logger;
		public IConfiguration Configuration { get; }


		public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment env)
		{
			Configuration = configuration;
			_logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			try
			{
				services.AddOptions();
				services.ConfigureConfigServerClientOptions(Configuration);
				services.AddConfiguration(Configuration);
				services.ConfigureCloudFoundryOptions(Configuration);

				services.AddMvc();
				services.Configure<ConfigurationSettings>(Configuration); 

				// You can access configuration in the startup code
				var appHost = Configuration["AppSettings:Host"];
				if (appHost == null) { _logger.LogWarning("Invalid configuration settings"); return; }
				_logger.LogInformation("Application host:: "+appHost);
      }
      catch (Exception ex)
      {
      	_logger.LogError(ex, "Startup/ConfigureServices()");
      }
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
			app.UseMvc();
    }

  }
}
