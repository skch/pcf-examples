
namespace RestApiShowcase.Models
{

	public class ConfigurationSettings
	{
		public ConfigAppSettings AppSettings { get; set; }	
	}

	public class ConfigAppSettings
	{
		public string Host { get; set; }

		private string microsoftApiKey;

		public string MicrosoftApiKey
		{
			get => SecureData.Decrypt(microsoftApiKey);
			set => microsoftApiKey = value;
		}
		
		public bool CacheDbResults { get; set; } = false;
		public int MaxStore { get; set; } = 100;
	}

}
