using IQVIAInterviewApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IQVIAInterviewApi.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0,0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2016, 1, 2, 0, 0, 0, TimeSpan.Zero);

            List<TweetModel> tweets = new List<TweetModel>();
            DateTimeOffset stop = startDate.AddYears(2);

            while (endDate <= stop)
            {
                List<TweetModel> tweetsInPeriod = await TweetManager.GetTweets(startDate, endDate);

                if(tweetsInPeriod.Count == 100)
                {
                    startDate = GetLastEndDateToStartFrom(startDate, endDate, tweetsInPeriod);
                }
                else
                {
                    startDate = endDate;
                    endDate = endDate.AddDays(10);
                }

                tweets.AddRange(tweetsInPeriod);
            }
            
            return View(tweets);
        }

        private static DateTimeOffset GetLastEndDateToStartFrom(DateTimeOffset startDate, DateTimeOffset endDate, List<TweetModel> tweetsInPeriod)
        {
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