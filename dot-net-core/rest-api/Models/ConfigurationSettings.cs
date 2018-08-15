
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
		public int MaxStore { get; set; } = 100;
	}

}
