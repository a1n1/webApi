using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Models;

namespace Web.Controllers
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
        public HttpResponseMessage Put(int id, ValueModel value)
        {
            //when call from test - you can see that value is populated.
            //do update data in some storage
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}