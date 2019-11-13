using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WorkApi.Controllers
{
    [Route("api/work")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet,Route("workForWorker")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "work for you:", "10h" };
        }
    }
}
