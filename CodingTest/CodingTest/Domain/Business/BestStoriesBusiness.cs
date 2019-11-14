﻿using CodingTest.API.Domain.Models;
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
        /// <summary>
        /// Get BestStories 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get Online BestStories or Cache BestStories
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// Get Online StoriesList
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        ///  Get Online StoriesDetais
        /// </summary>
        /// <param name="StoriesCodesList"></param>
        /// <returns></returns>
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

            ResultList = ResultList.Take(Cache.MainCache.AmountItens).OrderByDescending(x => x.score).ToList();
            
            Cache.CacheBestStories.Set(ResultList);

            return ResultList;
        }

        /// <summary>
        /// Handle with Time
        /// </summary>
        /// <param name="UnixTimeStr"></param>
        /// <returns></returns>
        private string handleTime( dynamic UnixTimeStr)
        {
            long LongTime;
            string strTime = UnixTimeStr.ToString() ?? "0";
            long.TryParse(strTime, out LongTime);
            string TimeFormated = UnixUnFormat(LongTime);
            return TimeFormated;
        }

        /// <summary>
        /// Format Time
        /// </summary>
        /// <param name="unixDateTime"></param>
        /// <returns></returns>
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
