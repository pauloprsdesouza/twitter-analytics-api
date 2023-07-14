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
using System;
using System.Text;

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

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                };

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

        [HttpPut, Route("update-scores")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> UpdateScores()
        {
            var tweets = await _tweetService.UpdateScores();

            return Ok();
        }

        [HttpGet, Route("all")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> FindAll()
        {
            var tweets = await _tweetService.FindAll();

            using var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                using var csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture);
                csvWriter.Context.RegisterClassMap<TweetResponseMap>();
                csvWriter.WriteRecords(tweets);
            }

            var csvContent = Encoding.UTF8.GetString(memoryStream.ToArray());
            var result = new FileContentResult(Encoding.UTF8.GetBytes(csvContent), "text/csv")
            {
                FileDownloadName = "tweets.csv",
            };

            return result;
        }
    }
}
