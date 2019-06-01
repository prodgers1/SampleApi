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
            DateTimeOffset startDate = new DateTimeOffset(2016, 1, 1, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset endDate = new DateTimeOffset(2018, 1, 1, 0, 0, 0, TimeSpan.Zero);
            List<TweetModel> tweets = await TweetManager.GetAll(startDate, endDate);
            return View(tweets);
        }

        



    }
}