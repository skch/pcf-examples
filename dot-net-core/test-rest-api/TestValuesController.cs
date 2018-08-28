
using System.Collections.Generic;
using RestApiShowcase.Controllers;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using RestApiShowcase.Models;
using Moq;

namespace test_rest_api
{
	public class TestValuesController
	{
		IConfigurationRoot config;
		IOptionsSnapshot<ConfigurationSettings > configServerData;
		IOptions<CloudFoundryApplicationOptions> appOptions;
		IOptions<CloudFoundryServicesOptions > servOptions;
		IOptions<ConfigServerClientSettingsOptions> confgServerSettings;

		private void initOptions()
		{
			var _configurationRoot = new Mock<IConfigurationRoot>();
			_configurationRoot.SetupGet(x => x[It.IsAny<string>()]).Returns("MOK1");
			config = _configurationRoot.Object;
				
			var options = new ConfigurationSettings(){ 
				AppSettings = new ConfigAppSettings { Host = "test-host"}
			};
			var configServerMock = new Mock<IOptionsSnapshot<ConfigurationSettings>>();
			configServerMock.Setup(m => m.Value).Returns(options);
			configServerData = configServerMock.Object;
			appOptions = new OptionsWrapper<CloudFoundryApplicationOptions>(new CloudFoundryApplicationOptions
			{
				Application_Id = "pcf-app-name"
			});
			servOptions = new OptionsWrapper<CloudFoundryServicesOptions>(new CloudFoundryServicesOptions());
			confgServerSettings = new OptionsWrapper<ConfigServerClientSettingsOptions>(new ConfigServerClientSettingsOptions
			{ Name = "config-server-name",
				Username = ""
			});
		}
		
		[Fact]
		public void ExchangeValues()
		{
			initOptions();
			var controller = new ValuesController(config, configServerData, appOptions, servOptions, confgServerSettings);
			
			string[] items = { "First", "Last" };
			controller.Post(items);
			
			var result = controller.Get();
			var viewResult = Assert.IsType<TestData>(result.Value);
			Assert.Equal("data", result.Value.status);
			Assert.Equal("pcf-app-name", result.Value.CFID);
			Assert.Equal("test-host", result.Value.environment);
			Assert.Equal("First", result.Value.message);
			Assert.Equal("config-server-name", result.Value.name);
			Assert.Equal("MOK1", result.Value.version);

			result = controller.Get();
			Assert.IsType<TestData>(result.Value);
			Assert.Equal("data", result.Value.status);
			Assert.Equal("Last", result.Value.message);

			result = controller.Get();
			Assert.IsType<TestData>(result.Value);
			Assert.Equal("error", result.Value.status);
		}


	}
}