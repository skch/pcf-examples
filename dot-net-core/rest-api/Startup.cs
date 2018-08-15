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
			app.UseMvc();
    }

  }
}
