using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IQVIAInterviewApi.Models
{
    public class TweetModel
    {
        [JsonProperty("id")]
        public decimal Id { get; set; }
        [JsonProperty("stamp")]
        public DateTimeOffset Stamp { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}