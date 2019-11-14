using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTest.API.Domain.Models
{
    /// <summary>
    /// Story Class
    /// </summary>
    public class Story
    {
        public string title { get; set; }
        public string url { get; set; }
        public string postedBy { get; set; }
        public string time { get; set; }
        public string score { get; set; }
        public string commentCount { get; set; }
    }

}
