using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rest2
{
	public class AppSettings
	{
		public string Host { get; set; }
		public string MicrosoftApiKey { get; set; }
		public bool CacheDbResults { get; set; } = false;
	}
}
