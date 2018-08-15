using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace RestApiShowcase.Controllers
{
  [Route("api/[controller]")]
  public class SentimentController : Controller
  {  
    private readonly AppSettings _appSettings;
    public SentimentController(IOptions<AppSettings> options)
    {
        _appSettings = options.Value;
    }


    public async Task<float> Get(string message)
    {
      var response = await MakeRequest(message, _appSettings.MicrosoftApiKey);
      if (response.IsSuccessStatusCode)
      {
        var data = await response.Content.ReadAsStringAsync();
        dynamic results = JsonConvert.DeserializeObject<dynamic>(data);
        return (float) results.documents[0].score.Value;
      }

      return 0;
    }

    private static async Task<HttpResponseMessage> MakeRequest(string msg, string key)
    {
      
      var client = new HttpClient();
      client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

      var uri = "https://westcentralus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";

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