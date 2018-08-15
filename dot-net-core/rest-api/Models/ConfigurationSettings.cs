using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiShowcase.Models
{
	public class ConfigurationSettings
	{
		public ConfigAppSettings AppSettings { get; set; }
	
	}


	public class ConfigAppSettings
	{
		public string Host { get; set; }
		public string MicrosoftApiKey { get; set; }
		public bool CacheDbResults { get; set; } = false;
	}

}
