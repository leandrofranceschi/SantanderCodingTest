using CodingTest.API.Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingTest.API.Cache
{



    public static class MainCache
    {
     
        public static int BestStoriesCacheSeconds;
        public static int AmountItens;
        public static string UrlBestStoriesList;
        public static string UrlStoryDetail;
        public static string TimeFormat;


        public static void Load()
        {
            UrlBestStoriesList = Startup.Configuration.GetSection("BestStories").GetSection("UrlBestStoriesList").Value;
            UrlStoryDetail = Startup.Configuration.GetSection("BestStories").GetSection("UrlStoryDetail").Value;
            TimeFormat = Startup.Configuration.GetSection("BestStories").GetSection("TimeFormat").Value;

            BestStoriesCacheSeconds = Convert.ToInt32(Startup.Configuration.GetSection("BestStories").GetSection("BestStoriesCacheSeconds").Value);
            AmountItens = Convert.ToInt32(Startup.Configuration.GetSection("BestStories").GetSection("AmountItens").Value);

        }    
    }

    public static class CacheBestStories
    {
        public static List<Story> BestStoriesList;
        public static DateTime DateTimeExpires;

        internal static void Set(List<Story> NewList)
        {
            BestStoriesList = NewList;
            DateTimeExpires = DateTime.Now.AddSeconds(MainCache.BestStoriesCacheSeconds);
        }
    }
}
