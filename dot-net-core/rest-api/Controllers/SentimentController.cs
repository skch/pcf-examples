using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestApiShowcase.Models;

namespace RestApiShowcase.Controllers
{
  [Route("api/[controller]")]
  public class SentimentController : Controller
  {  
    private IOptionsSnapshot<ConfigurationSettings> ConfigSettings { get; set; }
    private readonly ILogger<SentimentController> _logger;
    public SentimentController(ILogger<SentimentController> logger, 
							IOptionsSnapshot<ConfigurationSettings> configServerData)
    {
      _logger = logger;
			if (configServerData != null)
					ConfigSettings = configServerData;
    }


    public async Task<float> Get(string message)
    {
      string MSKEY = ConfigSettings.Value.AppSettings.MicrosoftApiKey;
			if (String.IsNullOrEmpty(MSKEY)) {
				_logger.LogWarning("Microsoft cognitive services are not configured for "+ConfigSettings.Value.AppSettings.Host);
				return -1;
			}
      
      //_logger.LogInformation("Use the decrypted key: "+MSKEY);
      
      var response = await MakeRequest(message, MSKEY);
      if (response.IsSuccessStatusCode)
      {
        var data = await response.Content.ReadAsStringAsync();
        dynamic results = JsonConvert.DeserializeObject<dynamic>(data);
        return (float) results.documents[0].score.Value;
      }

      _logger.LogWarning("Issues:: "+response.StatusCode);
      return -2;
    }

    private async Task<HttpResponseMessage> MakeRequest(string msg, string key)
    {
      
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

      var uri = ConfigSettings.Value.AppSettings.MicrosoftUri;

      HttpResponseMessage response;

      var byteData =
        Encoding.UTF8.GetBytes("{'documents': [{'language': 'en','id': 'string','text': '" + msg + "'}]}");

      using (var content = new ByteArrayContent(byteData))
      {
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        response = await client.PostAsync(uri, content);
        return response;
      }
    }
  }
}