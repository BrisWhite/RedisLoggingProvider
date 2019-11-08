using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace RedisLoggingProvider.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logger.LogInformation("this is an {type} information from {CompanyCode}", "new", "Microsoft");

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            if (id == 0)
            {
                using (var scope = _logger.BeginScope(""))
                {
                    _logger.LogInformation("haha");
                    _logger.LogInformation("xixi");
                }
            }
            else if (id == 1)
            {
                try
                {
                    var hah = "xx";
                    double.Parse(hah);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "double parese error");
                }

                _logger.LogError(new EventId(100), new Exception("there are new exception"), "Get {id} gotta error", id);
            }

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
