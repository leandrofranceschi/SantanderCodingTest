using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodingTest.API.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CodingTest.API.Controllers
{

    /// <summary>
    /// List Best Stories with details
    /// </summary>
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

    
}
