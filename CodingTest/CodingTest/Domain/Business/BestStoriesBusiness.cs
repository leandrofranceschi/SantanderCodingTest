using CodingTest.API.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodingTest.API.Domain.Business
{
    public class BestStories
    {
        public async Task<List<Story>> GetAsync()
        {
            

            //Cache is Expired
            if (DateTime.Now > Cache.CacheBestStories.DateTimeExpires)
            {
                return await BestStoriesGetOnlineAsync();
            }
            else
            {
                return Cache.CacheBestStories.BestStoriesList;
            }


        }

        private async Task<List<Story>> BestStoriesGetOnlineAsync()
        {
            List<Story> result = new List<Story>();

            try
            {
                List<int> StoriesList = await GetStoriesList(); 
                
                // if do not get a list of Best Stories
                if( StoriesList.Count < Cache.MainCache.AmountItens)
                   return result;

                result = await GetStoriesDetaisAsync(StoriesList);

                return result;
            }
            catch( Exception e)
            {
                return result;
            }
        }



        private async Task<List<int>> GetStoriesList()
        {
            List<int> ResultList = new List<int>();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(Cache.MainCache.UrlBestStoriesList))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ResultList = JsonConvert.DeserializeObject<List<int>>(apiResponse);
                }
            }
            return ResultList.Take(Cache.MainCache.AmountItens).ToList();
        }

        private async Task<List<Story>> GetStoriesDetaisAsync(List<int> StoriesCodesList)
        {
            
            List<Story> ResultList = new List<Story>();

            foreach (int Code in StoriesCodesList)
            {

                string url = string.Format(Cache.MainCache.UrlStoryDetail, Code);

                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        dynamic Detail = JsonConvert.DeserializeObject<dynamic>(apiResponse);

                        Story Story = new Story()
                        {
                            title = Detail.title ?? "",
                            url = Detail.url ?? "",
                            postedBy = Detail.postedBy ?? "",
                            time = handleTime(Detail.time),
                            score = Detail.score ?? "",
                            commentCount = Detail.commentCount ?? ""
                        };

                        ResultList.Add(Story);
                    }
                }

            }
            Cache.CacheBestStories.Set(ResultList);

            return ResultList.Take(Cache.MainCache.AmountItens).ToList();
        }

        private string handleTime( dynamic UnixTimeStr)
        {
            long LongTime;
            string strTime = UnixTimeStr.ToString() ?? "0";
            long.TryParse(strTime, out LongTime);
            string TimeFormated = UnixUnFormat(LongTime);
            return TimeFormated;
        }

        private string UnixUnFormat(long unixDateTime)
        {
            //System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

            //DateTime localDateTimeOffset = dateTime.AddSeconds(unixDateTime);
            DateTime localDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixDateTime).DateTime.ToLocalTime();

            string strResult = localDateTimeOffset.ToString(Cache.MainCache.TimeFormat);

            return strResult;
        }
    }
}
