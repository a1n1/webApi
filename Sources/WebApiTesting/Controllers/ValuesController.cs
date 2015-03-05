using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApiTesting.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public HttpResponseMessage Put(int id, [FromBody] string value)
        {
            var zzz = 5;
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}