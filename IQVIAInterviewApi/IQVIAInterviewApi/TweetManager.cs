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
    public class TweetManager
    {
        private static HttpClient m_client = new HttpClient() { BaseAddress = new Uri("https://badapi.iqvia.io/api/v1/Tweets") };

        public static async Task<List<TweetModel>> GetTweets(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string query = $"?startDate={startDate.ToString("u")}&endDate={endDate.ToString("u")}";
            var response = await m_client.GetAsync(query);
            string content = await response.Content.ReadAsStringAsync();

            List<TweetModel> models = JsonConvert.DeserializeObject<List<TweetModel>>(content);
            return models;
        }
    }
}