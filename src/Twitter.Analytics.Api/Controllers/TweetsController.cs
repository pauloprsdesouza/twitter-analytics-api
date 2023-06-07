using System.Linq;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.Tweets.Models;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/tweets")]
    public class TweetsController : Controller
    {
        private readonly ITweetService _tweetService;

        public TweetsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpPost, Route("csv")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> SaveTweetsFromCsv(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture);

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<TweetMap>();
                    var tweets = csv.GetRecords<Tweet>().ToList();

                    await _tweetService.CreateFromList(tweets);

                    return Ok(tweets);
                }
            }
            catch (System.Exception ex)
            {
                return NoContent();
            }
        }

        [HttpPost, Route("extract-from-unprocessed-users")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ExtractPublishedTweets()
        {
            var tweets = await _tweetService.GetRepliesByAccount("12354");

            return Ok();
        }
    }
}
