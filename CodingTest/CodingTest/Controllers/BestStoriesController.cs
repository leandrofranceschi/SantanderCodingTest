using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingTest.API.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CodingTest.API.Controllers
{

 
    [Route("api/v{version:apiVersion}/BestStories")]
    [ApiController]
    public class BestStoriesController : Controller
    {
        
        [HttpGet]
        public ActionResult<List<Story>>Get()
        {
            List<Story> result = new List<Story>();

            result = new Domain.Business.BestStories().GetAsync().Result;

            if (result.Count >= Cache.MainCache.AmountItens)
                return Ok(result);

            return NotFound(result);
                
        }
    }

    /*
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
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
    */
}
