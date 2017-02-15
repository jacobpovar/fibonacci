using System.Collections.Generic;
using System.Web.Http;

namespace Fibonacci.Initiator
{
    public class ValuesController : ApiController
    {
        // GET api/values 
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}