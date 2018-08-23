namespace RestApiShowcase.Models
{
	public class AppVault
	{
		private static readonly string privateKey =
		  "%PRIVATEKEY%";


		public static string getPrivateKey()
		{
			return privateKey;
		}
		
	}
}