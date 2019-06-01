using IQVIAInterviewApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace IQVIAInterviewApi
{
    //this is acting as my BusinessLogic layer, ideally this would be in a separate project and my web app would reference this
    //but for the sake of this small example, i just made a class that acts as that layer.
    public class TweetManager
    {
        private static HttpClient m_client = new HttpClient() { BaseAddress = new Uri("https://badapi.iqvia.io/api/v1/Tweets") };

        //Proxy to call the external API
        public static async Task<List<TweetModel>> GetTweets(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            //convert to UTC time
            string query = $"?startDate={startDate.ToString("u")}&endDate={endDate.ToString("u")}";
            var response = await m_client.GetAsync(query);
            string content = await response.Content.ReadAsStringAsync();

            //convert the json into models and set the properties to be used in the view
            List<TweetModel> models = JsonConvert.DeserializeObject<List<TweetModel>>(content);
            return models;
        }

        /// <summary>
        /// Returns a list of tweets for a given time range. Ensures no duplicates are returned.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static async Task<List<TweetModel>> GetAll(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            List<TweetModel> tweets = new List<TweetModel>();
            
            while (startDate <= endDate)
            {
                List<TweetModel> tweetsInPeriod = await TweetManager.GetTweets(startDate, endDate);

                if (tweetsInPeriod.Count == 100)
                {
                    startDate = GetLastEndDateToStartFrom(startDate, endDate, tweetsInPeriod);
                }
                else
                {
                    startDate = startDate.AddMonths(1);
                }

                tweets.AddRange(tweetsInPeriod);
            }

            return tweets;
        }

        //helper method to ensure I get all the records if there were 100 in the returned query.
        private static DateTimeOffset GetLastEndDateToStartFrom(DateTimeOffset startDate, DateTimeOffset endDate, List<TweetModel> tweetsInPeriod)
        {
            //need to get the time of the last tweet to start my next query from if I received back 100.
            //There could or could not be more after this, so set the start date to the last tweet's time
            //so i can check from that point forward to ensure i dont miss any tweets.
            //Also, remove it from the list so i avoid duplicates.
            TweetModel lastTweet = tweetsInPeriod.Last();
            if (lastTweet.Stamp < endDate)
            {
                startDate = lastTweet.Stamp;
                tweetsInPeriod.Remove(lastTweet);

            }
            return startDate;
        }
    }
}