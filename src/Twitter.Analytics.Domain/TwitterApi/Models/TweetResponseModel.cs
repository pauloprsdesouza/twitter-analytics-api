using System.Collections.Generic;
using System.Linq;
using Twitter.Analytics.Domain.TwitterApi.Models.Tweets;

namespace Twitter.Analytics.Domain.TwitterApi.Models
{
    public class TweetResponseModel
    {
        public List<TweetModel> Data { get; set; }
        public IncludeResponseModel Includes { get; set; }

        public List<TweetModel> GetTweets()
        {
            foreach (var item in Data)
            {
                item.Author =  Includes.Users.SingleOrDefault(y => y.Id == item.AuthorId);
            }

            return Data;
        }
    }
}
