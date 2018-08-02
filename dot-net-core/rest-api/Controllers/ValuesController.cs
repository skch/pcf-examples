using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using rest2.Models;

namespace rest2.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private static ConcurrentQueue<string> cq = new ConcurrentQueue<string>();
    private readonly AppSettings _appSettings;
    public ValuesController(IOptions<AppSettings> options)
    {
      _appSettings = options.Value;
    }

    private TestData createResponse(string name, string value) {
      return new TestData { environment = _appSettings.Host, status = name, message = value };
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
			foreach (string value in data) {
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
