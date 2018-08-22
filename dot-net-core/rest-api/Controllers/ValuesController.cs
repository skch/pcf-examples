using System;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using RestApiShowcase.Models;

namespace RestApiShowcase.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
		private IOptionsSnapshot<ConfigurationSettings> ConfigSettings { get; set; }
		private CloudFoundryServicesOptions CloudFoundryServices { get; set; }
		private CloudFoundryApplicationOptions CloudFoundryApplication { get; set; }
		private ConfigServerClientSettingsOptions ConfigServerClientSettingsOptions { get; set; }
		private IConfiguration Configuration { get; }
    private static ConcurrentQueue<string> cq = new ConcurrentQueue<string>();
    
    public ValuesController(IConfigurationRoot config,
            IOptionsSnapshot<ConfigurationSettings> configServerData,
            IOptions<CloudFoundryApplicationOptions> appOptions,
            IOptions<CloudFoundryServicesOptions> servOptions,
            IOptions<ConfigServerClientSettingsOptions> confgServerSettings)
    {
      // The ASP.NET DI mechanism injects the data retrieved from the
			// Spring Cloud Config Server as an IOptionsSnapshot<ConfigServerData>
			// since we added "services.Configure<ConfigServerData>(Configuration);"
			// in the StartUp class
			if (configServerData != null)
					ConfigSettings = configServerData;

			// The ASP.NET DI mechanism injects these as well, see
			// public void ConfigureServices(IServiceCollection services) in Startup class
			if (servOptions != null)
					CloudFoundryServices = servOptions.Value;
			if (appOptions != null)
					CloudFoundryApplication = appOptions.Value;

			// Inject the settings used in communicating with the Spring Cloud Config Server
			if (confgServerSettings != null)
					ConfigServerClientSettingsOptions = confgServerSettings.Value;

			Configuration = config;
    }

    private TestData createResponse(string name, string value)
    {
      return new TestData { 
				environment = ConfigSettings.Value.AppSettings.Host, 
				name = ConfigServerClientSettingsOptions.Name,
				version = Configuration["version"],
				CFID = CloudFoundryApplication.Application_Id,
				status = name, 
				message = value 
			};
    }

    // GET api/values
    [HttpGet]
    public ActionResult<TestData> Get()
    {
			string data;
			if (!cq.TryPeek(out data)) return createResponse("error", "Queue is empty");
			if (!cq.TryDequeue(out data)) return createResponse("error", "Queue is empty now");
      return createResponse("data", data);
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public ActionResult<TestData> Get(string id)
    {
			cq.Enqueue(id);
			return createResponse("size", cq.Count.ToString());
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string[] data)
    {
	    int max_count = ConfigSettings.Value.AppSettings.MaxStore;
			foreach (string value in data) {
				if (cq.Count >= max_count) break;
				cq.Enqueue(value);
			}
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
