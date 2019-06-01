using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using IQVIAInterviewApi;
using IQVIAInterviewApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task GetJanuary2016()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2016, 1, 31, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);

            Assert.AreEqual(473, tweets.Count);

        }

        [TestMethod]
        public async Task CheckTotalAndEnsureNoDuplicates()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2018, 1, 1, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);

            //found this by going one day at a time initially to find total number of tweets i should be expecting.
            Assert.AreEqual(11693, tweets.Count);

            IEnumerable<IGrouping<decimal, TweetModel>> grouped = tweets.GroupBy(t => t.Id);

            //there are not any groups with the same ids that have more than one in the group
            Assert.IsTrue(!grouped.Any(g => g.Count() > 1));
        }

        [TestMethod]
        public async Task CheckJsonParser()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2016, 1, 2, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);

            foreach (TweetModel tweet in tweets)
            {
                Assert.IsNotNull(tweet.Id);
                Assert.IsNotNull(tweet.Text);
                Assert.IsNotNull(tweet.Stamp);
            }

        }
        [TestMethod]
        public async Task EnsureNotGettingTweetsOutsideDateRange()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2016, 1, 3, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);

            IOrderedEnumerable<TweetModel> ordered = tweets.OrderBy(t => t.Stamp);

            Assert.IsTrue(ordered.Last().Stamp <= endDate);
            Assert.IsTrue(ordered.First().Stamp >= startDate);
        }

        [TestMethod]
        public async Task CheckGettingOver100IsStillInValidRange()
        {
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 2, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2016, 1, 12, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);

            IOrderedEnumerable<TweetModel> ordered = tweets.OrderBy(t => t.Stamp);

            Assert.IsTrue(tweets.Count > 100);
            Assert.IsTrue(ordered.Last().Stamp <= endDate);
            Assert.IsTrue(ordered.First().Stamp >= startDate);
        }

    }
}
