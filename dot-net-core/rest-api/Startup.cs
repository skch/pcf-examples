using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pivotal.Discovery.Client;
//using Steeltoe.Extensions.Configuration;
//using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Management.CloudFoundry;
using Steeltoe.Management.Endpoint.Health;

namespace rest2
{
	public class Startup
	{
		private readonly ILogger<Startup> _logger;
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, ILogger<Startup> logger)
		{
			Configuration = configuration;
			_logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			try
			{
				services.AddCloudFoundryActuators(Configuration);
				services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
				services.AddMvc();
        services.AddSingleton(Configuration);  
      }
      catch (Exception ex)
      {
      	_logger.LogError(ex, "Startup.cs - ConfigureServices() error ");
      }
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
    	try
    	{
    		app.UseMvc();
    	}
    	catch (Exception ex)
    	{
    		_logger.LogError(ex, "Startup.cs - Configure() error ");
    	}
    }

  }
}
