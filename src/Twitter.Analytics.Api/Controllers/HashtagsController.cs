using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Analytics.Domain.Hashtags.Models;
using Twitter.Analytics.Domain.Tweets;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/hashtags")]
    public class HashtagsController : Controller
    {
        private readonly ITweetService _tweetService;

        public HashtagsController(ITweetService tweetService)
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
                    csv.Context.RegisterClassMap<HashtagMap>();
                    var hashtags = csv.GetRecords<HashtagModel>().ToList();

                    await _tweetService.UpdateHashtags(hashtags);

                    return Ok();
                }
            }
            catch (System.Exception ex)
            {
                return NoContent();

            }
        }
    }
}
